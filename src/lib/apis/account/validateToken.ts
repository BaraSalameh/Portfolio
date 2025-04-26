import { createAsyncThunk } from '@reduxjs/toolkit';

export const validateToken = createAsyncThunk(
    'auth/validateToken',
    async (_, thunkAPI) => {
        try {
            const response = await fetch(`https://localhost:7206/api/Account/ValidateToken`,{
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                credentials: 'include',
                body: JSON.stringify({}),
            });

            const data = await response.json();

            if(!data.status){
                return thunkAPI.rejectWithValue({
                    error: 'data.lstError || "Unknown error occured"'
                });
            }
    
            return data;

        } catch (error) {
            return thunkAPI.rejectWithValue('Unauthorized');
        }
    }
);
