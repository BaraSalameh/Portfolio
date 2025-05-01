import { createAsyncThunk } from '@reduxjs/toolkit';

export const validateToken = createAsyncThunk(
    'auth/validateToken',
    async (_, thunkAPI) => {
        const res = await fetch(`https://localhost:7206/api/Account/ValidateToken`,{
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            credentials: 'include',
            body: JSON.stringify({}),
        });

        if (res.status === 401){
            const error = await res.json();
            return thunkAPI.rejectWithValue(error);
        }

        if (!res.ok) {
            return thunkAPI.rejectWithValue(["Unexpected error occurred"]);
        }

        const data = await res.json();
        return data;
    }
);
