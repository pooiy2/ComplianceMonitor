# Ease frontend

The frontend is a React + TypeScript single-page application built with Vite.

## Run locally

1. Start the Ease API on `https://localhost:7089` using the HTTPS launch profile.
2. From this directory, run `npm install` and `npm run dev`.
3. Open the local URL printed by Vite (by default, `http://127.0.0.1:5173`).

The frontend calls `/Summary`, `/Analyze`, and `/History` on `https://localhost:7089` by default. Set `VITE_API_BASE_URL` before starting Vite to use a different API origin. The backend CORS policy permits the default Vite development origins (`http://localhost:5173` and `http://127.0.0.1:5173`).
