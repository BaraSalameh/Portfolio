import { createAsyncThunk } from '@reduxjs/toolkit';

export const userListQuery = createAsyncThunk(
    'search/userListQuery',
    async (query: string) => {

        const res = await fetch(`https://localhost:7206/api/Client/UserList?Search=${query}`);

        if (res.status === 204) return {items: []};
        
        return await res.json();
    }
);
