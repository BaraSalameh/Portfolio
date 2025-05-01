import { createAsyncThunk } from "@reduxjs/toolkit";

export const confirmEmail = createAsyncThunk(
    'auth/confirmEmail',
    async (payload: { email: string; token: string }, thunkAPI) => {
        try {
            const query = new URLSearchParams({
                email: payload.email,
                token: payload.token
            }).toString();
    
            const res = await fetch(`https://localhost:7206/api/Account/ConfirmEmail?${query}`, {
                method: 'GET',
                credentials: 'include'
            });
    
            if (res.status === 204) return {isConfirmed: true};
        
            if (!res.ok){
                const error = await res.json();
                return thunkAPI.rejectWithValue(error)
            }

        } catch (error) {
            return thunkAPI.rejectWithValue(['Unexpected error occurred.']);
        }
    }
);
  