import { createAsyncThunk } from "@reduxjs/toolkit";

export const login = createAsyncThunk(
    'auth/login',
    async (payload: { email: string; password: string, rememberMe: boolean }, thunkAPI) => {
        try {
            const response = await fetch('https://localhost:7206/api/Account/Login', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(payload),
                credentials: 'include'
            });

            const data = await response.json();

            if(!data.status){
                return thunkAPI.rejectWithValue({
                    error: data.lstError
                });
            }
    
            return data;

        } catch (error) {
            return thunkAPI.rejectWithValue('Network error');
        }
    }
);