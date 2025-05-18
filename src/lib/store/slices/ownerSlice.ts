import { logout } from '@/lib/apis/account/logout';
import { userInfoQuery } from '@/lib/apis/owner/user/userInfoQuery';
import { userQuery } from '@/lib/apis/owner/userQuery';
import { createSlice } from '@reduxjs/toolkit';

const ownerSlice = createSlice({
    name: 'owner',
    initialState: {
        user: null,
        loading: false,
        error: null as string | null,
    },
    reducers: {},
    extraReducers: (builder) => {
        builder
        .addCase(userQuery.pending, (state) => {
            state.loading = true;
            state.error = null;
        })
        .addCase(userQuery.fulfilled, (state, action) => {
            state.loading = false;
            state.user = action.payload.user;
        })
        .addCase(userQuery.rejected, (state, action) => {
            state.loading = false;
            state.error = (action.payload as string);
        })
        
        .addCase(userInfoQuery.pending, (state) => {
            state.loading = true;
            state.error = null;
        })
        .addCase(userInfoQuery.fulfilled, (state, action) => {
            state.loading = false;
            state.user = action.payload;
        })
        .addCase(userInfoQuery.rejected, (state, action) => {
            state.loading = false;
            state.error = (action.payload as string);
        })
        
        .addCase(logout.pending, (state) => {
            state.loading = true;
            state.error = null;
        })
        .addCase(logout.fulfilled, () => {
            return{
                loading: false,
                error: null,
                user: null
            }
        })
        .addCase(logout.rejected, (_, action) => {
            return {
                user: null,
                loading: false,
                error: action.payload as string,
            }
        })
    },
});
export default ownerSlice.reducer;
