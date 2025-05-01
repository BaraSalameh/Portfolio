import { createAsyncThunk } from "@reduxjs/toolkit";

export const register = createAsyncThunk(
    'auth/register',
    async (payload: { firstname: string, lastname: string, email: string; password: string, rememberMe: boolean }, thunkAPI) => {
        try {
            const response = await fetch('https://localhost:7206/api/Account/Register', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(payload),
                credentials: 'include'
            });

            if(!response.ok){
                const error = await response.json();
                return thunkAPI.rejectWithValue(error);
            }

            const data = await response.json();
            return {...data, isConfirmed: false};

        } catch (error) {
            return thunkAPI.rejectWithValue(['Unexpected error occurred.']);
        }
    }
);