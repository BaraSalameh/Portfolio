import { createAsyncThunk } from '@reduxjs/toolkit';

export const userListQuery = createAsyncThunk(
    'search/userListQuery',
    async (query: string) => {
        // const pageNumber = Math.floor(skip / take) + 1;
        // const pageSize = take;

        const res = await fetch(`https://localhost:7206/api/Client/UsersList?Search=${query}`);

        if (!res.ok) {
            const error = await res.json();
            throw new Error(error?.message || 'Failed to fetch user list');
        }

        const data = await res.json();
        return data;
    }
);
