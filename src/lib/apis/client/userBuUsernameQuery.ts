import { createAsyncThunk } from '@reduxjs/toolkit';

export const userByUsernameQuery = createAsyncThunk(
    'user/userByUsernameQuery',
    async (username: string, thunkAPI) => {
        try {
            const res = await fetch(`https://localhost:7206/api/Client/UserByUsername?Username=${username}`);

            if (!res.ok) {
                const error = await res.json();
                return thunkAPI.rejectWithValue(error);
            }

            const data = await res.json();
        return data;
        } catch {
            return thunkAPI.rejectWithValue(['Unexpected error occurred']);
        }
    }
);
