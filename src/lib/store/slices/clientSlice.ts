import { userByUsernameQuery } from '@/lib/apis/client/userBuUsernameQuery';
import { ProfileFormData } from '@/lib/schemas/profileSchema';
import { createSlice } from '@reduxjs/toolkit';

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

const clientSlice = createSlice({
    name: 'client',
    initialState,
    reducers: {},
    extraReducers: (builder) => {
        builder
        .addCase(userByUsernameQuery.pending, (state) => {
            state.loading = true;
            state.error = null;
        })
        .addCase(userByUsernameQuery.fulfilled, (state, action) => {
            state.loading = false;
            state.user = action.payload.user;
        })
        .addCase(userByUsernameQuery.rejected, (state, action) => {
            state.loading = false;
            state.error = (action.payload as string);
        });
    },
});
export default clientSlice.reducer;
