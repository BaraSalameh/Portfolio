import { createAsyncThunk } from '@reduxjs/toolkit';

export const userByUsernameQuery = createAsyncThunk(
    'user/userByUsernameQuery',
    async (username: string) => {

        const res = await fetch(`https://localhost:7206/api/Client/UserByUsername?Username=${username}`);

        if (!res.ok) {
            const error = await res.json();
            throw new Error(error?.message || 'Failed to fetch user');
        }

        const data = await res.json();
        return data;
    }
);
