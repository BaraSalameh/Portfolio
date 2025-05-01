import { createSlice } from '@reduxjs/toolkit';
import { userListQuery } from '@/lib/apis/client/userListQuery';

const searchSlice = createSlice({
    name: 'search',
    initialState: {
        userList: [],
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
            state.userList = action.payload.items;
        })
        .addCase(userListQuery.rejected, (state) => {
            state.loading = false;
            state.error = 'Unexpected error occurred';
        });
    },
});

export default searchSlice.reducer;
