import { createAsyncThunk } from '@reduxjs/toolkit';

export const userByUsernameQuery = createAsyncThunk(
    'user/userByUsernameQuery',
    async (username: string, thunkAPI) => {

        const res = await fetch(`https://localhost:7206/api/Client/UserByUsername?Username=${username}`);

        if (res.status === 400) {
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
