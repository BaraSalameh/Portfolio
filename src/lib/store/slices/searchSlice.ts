import { createSlice } from '@reduxjs/toolkit';
import { userListQuery } from '@/lib/apis/client/userListQuery';

const searchSlice = createSlice({
    name: 'search',
    initialState: {
        users: [],
        loading: false,
        error: null as string | null,
    },
    reducers: {},
    extraReducers: (builder) => {
        builder
        .addCase(userListQuery.pending, (state) => {
            state.loading = true;
            state.error = null;
        })
        .addCase(userListQuery.fulfilled, (state, action) => {
            state.loading = false;
            state.users = action.payload.items;
        })
        .addCase(userListQuery.rejected, (state, action) => {
            state.loading = false;
            state.error = action.error.message || 'Search failed';
        });
    },
});

export default searchSlice.reducer;
