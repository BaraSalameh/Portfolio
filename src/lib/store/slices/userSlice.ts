import { userByUsernameQuery } from '@/lib/apis/client/userBuUsernameQuery';
import { createSlice } from '@reduxjs/toolkit';

const userSlice = createSlice({
    name: 'user',
    initialState: {
        user: null,
        loading: false,
        error: null as string | null,
    },
    reducers: {},
    extraReducers: (builder) => {
        builder
        .addCase(userByUsernameQuery.pending, (state) => {
            state.loading = true;
            state.error = null;
        })
        .addCase(userByUsernameQuery.fulfilled, (state, action) => {
            state.loading = false;
            state.user = action.payload;
        })
        .addCase(userByUsernameQuery.rejected, (state, action) => {
            state.loading = false;
            state.error = (action.payload as string);
        });
    },
});
export default userSlice.reducer;
