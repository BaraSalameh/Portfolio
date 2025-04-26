import { createAsyncThunk } from "@reduxjs/toolkit";

export const resendEmail = createAsyncThunk(
    'auth/resendEmail',
    async (payload: { email: string }, thunkAPI) => {
        try {
            const query = new URLSearchParams({
                email: payload.email
            }).toString();
    
            const response = await fetch(`https://localhost:7206/api/Account/ResendConfirmEmail?${query}`, {
                method: 'GET',
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
  