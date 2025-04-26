import { createAsyncThunk } from "@reduxjs/toolkit";

export const confirmEmail = createAsyncThunk(
    'auth/confirmEmail',
    async (payload: { email: string; token: string }, thunkAPI) => {
        try {
            const query = new URLSearchParams({
                email: payload.email,
                token: payload.token
            }).toString();
    
            const response = await fetch(`https://localhost:7206/api/Account/ConfirmEmail?${query}`, {
                method: 'GET',
                credentials: 'include'
            });
    
            const data = await response.json();
    
            if (!data.status) {
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
  