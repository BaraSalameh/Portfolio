import { createAsyncThunk } from "@reduxjs/toolkit";
import { useRouter } from "next/navigation";

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