"""
Tests d'intégration et d'authentification pour le router users_router
"""

import pytest
from httpx import AsyncClient
from fastapi import status
from main import app

import asyncio

@pytest.mark.asyncio
async def test_register_and_login():
	async with AsyncClient(app=app, base_url="http://test") as ac:
		# Test d'inscription
		register_data = {"email": "testuser@example.com", "password": "testpass"}
		resp = await ac.post("/users/register", json=register_data)
		assert resp.status_code == status.HTTP_200_OK
		user = resp.json()
		assert user["email"] == "testuser@example.com"

		# Test de login
		login_data = {"email": "testuser@example.com", "password": "testpass"}
		resp = await ac.post("/users/login", json=login_data)
		assert resp.status_code == status.HTTP_200_OK
		assert "access_token" in resp.cookies

@pytest.mark.asyncio
async def test_auth_required_for_user_info():
	async with AsyncClient(app=app, base_url="http://test") as ac:
		# Sans authentification
		resp = await ac.get("/users/1")
		assert resp.status_code == status.HTTP_401_UNAUTHORIZED or resp.status_code == status.HTTP_403_FORBIDDEN

		# Avec authentification
		# D'abord inscription et login
		register_data = {"email": "authuser@example.com", "password": "authpass"}
		await ac.post("/users/register", json=register_data)
		login_data = {"email": "authuser@example.com", "password": "authpass"}
		resp = await ac.post("/users/login", json=login_data)
		assert "access_token" in resp.cookies
		cookies = resp.cookies
		# Récupération de l'utilisateur connecté (id supposé 2)
		resp = await ac.get("/users/2", cookies=cookies)
		assert resp.status_code == status.HTTP_200_OK
		user = resp.json()
		assert user["email"] == "authuser@example.com"

@pytest.mark.asyncio
async def test_logout():
    async with AsyncClient(app=app, base_url="http://test") as ac:
        # Inscription et login
        await ac.post("/users/register", json={"email": "logoutuser@example.com", "password": "logoutpass"})
        resp = await ac.post("/users/login", json={"email": "logoutuser@example.com", "password": "logoutpass"})
        cookies = resp.cookies
        # Logout
        resp = await ac.post("/users/logout", cookies=cookies)
        assert resp.status_code == status.HTTP_200_OK
        assert resp.json()["message"] == "Déconnecté"

@pytest.mark.asyncio
async def test_read_users():
    async with AsyncClient(app=app, base_url="http://test") as ac:
        # Inscription et login
        await ac.post("/users/register", json={"email": "listuser@example.com", "password": "listpass"})
        resp = await ac.post("/users/login", json={"email": "listuser@example.com", "password": "listpass"})
        cookies = resp.cookies
        # Lecture de tous les utilisateurs
        resp = await ac.get("/users/", cookies=cookies)
        assert resp.status_code in [status.HTTP_200_OK, status.HTTP_403_FORBIDDEN]  # Peut être limité par les droits

@pytest.mark.asyncio
async def test_update_and_delete_user():
    async with AsyncClient(app=app, base_url="http://test") as ac:
        # Inscription et login
        await ac.post("/users/register", json={"email": "updateuser@example.com", "password": "updatepass"})
        resp = await ac.post("/users/login", json={"email": "updateuser@example.com", "password": "updatepass"})
        cookies = resp.cookies
        # Mise à jour utilisateur (id supposé 5)
        update_data = {"email": "updateuser2@example.com", "password": "updatepass2", "address": "Paris"}
        resp = await ac.put("/users/5", json=update_data, cookies=cookies)
        assert resp.status_code in [status.HTTP_200_OK, status.HTTP_403_FORBIDDEN]
        # Suppression utilisateur
        resp = await ac.delete("/users/5", cookies=cookies)
        assert resp.status_code in [status.HTTP_200_OK, status.HTTP_403_FORBIDDEN]

@pytest.mark.asyncio
async def test_sessions_and_evaluations():
    async with AsyncClient(app=app, base_url="http://test") as ac:
        # Inscription et login
        await ac.post("/users/register", json={"email": "sessionuser@example.com", "password": "sessionpass"})
        resp = await ac.post("/users/login", json={"email": "sessionuser@example.com", "password": "sessionpass"})
        cookies = resp.cookies
        # Lecture des sessions (id supposé 6)
        resp = await ac.get("/users/6/sessions", cookies=cookies)
        assert resp.status_code in [status.HTTP_200_OK, status.HTTP_403_FORBIDDEN]
        # Lecture des évaluations
        resp = await ac.get("/users/6/evaluations", cookies=cookies)
        assert resp.status_code in [status.HTTP_200_OK, status.HTTP_403_FORBIDDEN]
