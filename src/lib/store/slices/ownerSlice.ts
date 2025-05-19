import { logout } from '@/lib/apis/account/logout';
import { userInfoQuery } from '@/lib/apis/owner/user/userInfoQuery';
import { createSlice } from '@reduxjs/toolkit';
import { ProfileFormData } from "@/lib/schemas/profileSchema";
import { userFullInfoQuery } from '@/lib/apis/owner/user/userFullInfoQuery';

interface ProfileState {
    user: ProfileFormData | null;
    loading: boolean;
    error: string[] | string | null;
}

const initialState: ProfileState = {
    user: null,
    loading: false,
    error: null,
};

const ownerSlice = createSlice({
    name: 'owner',
    initialState: initialState,
    reducers: {},
    extraReducers: (builder) => {
        builder
        .addCase(userFullInfoQuery.pending, (state) => {
            state.loading = true;
            state.error = null;
        })
        .addCase(userFullInfoQuery.fulfilled, (state, action) => {
            state.loading = false;
            state.user = action.payload.user;
        })
        .addCase(userFullInfoQuery.rejected, (state, action) => {
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
