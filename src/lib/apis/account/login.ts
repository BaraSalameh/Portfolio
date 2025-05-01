import { createAsyncThunk } from "@reduxjs/toolkit";

export const login = createAsyncThunk(
    'auth/login',
    async (payload: { email: string; password: string, rememberMe: boolean }, thunkAPI) => {
        try {
            const res = await fetch('https://localhost:7206/api/Account/Login', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(payload),
                credentials: 'include'
            });

            if (res.status === 404){
                var error = await res.json();
                return thunkAPI.rejectWithValue(error);
            }

            if (res.status === 403){
                var error = await res.json();
                return thunkAPI.rejectWithValue(false);
            }
            const data = await res.json();

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