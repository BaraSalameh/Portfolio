import { createAsyncThunk } from "@reduxjs/toolkit";

export const logout = createAsyncThunk(
    'auth/logout',
    async (_, thunkAPI) => {
        try {
            const response = await fetch('https://localhost:7206/api/Account/Logout', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({}),
                credentials: "include"
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