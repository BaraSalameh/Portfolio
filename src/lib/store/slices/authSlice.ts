import { confirmEmail } from '@/lib/apis/account/confirmEmail';
import { login } from '@/lib/apis/account/login';
import { logout } from '@/lib/apis/account/logout';
import { register } from '@/lib/apis/account/register';
import { resendEmail } from '@/lib/apis/account/resendEmail';
import { validateToken } from '@/lib/apis/account/validateToken';
import { createSlice } from '@reduxjs/toolkit';

const authSlice = createSlice({
    name: 'auth',
    initialState: {
        username: null,
        role: null,
        isConfirmed: null,
        loading: false,
        error: null as string | null,
    },
    reducers: {},
    extraReducers: (builder) => {
        builder
        .addCase(login.pending, (state) => {
            state.loading = true;
            state.error = null;
        })
        .addCase(login.fulfilled, (state, action) => {
            state.loading = false;
            state.username = action.payload.username;
            state.role = action.payload.role;
            state.isConfirmed = action.payload.isConfirmed;
        })
        .addCase(login.rejected, (state, action) => {
            state.loading = false;
            state.error = (action.payload as any)?.error || 'Login failed';
        })

        .addCase(register.pending, (state) => {
            state.loading = true;
            state.error = null;
        })
        .addCase(register.fulfilled, (state, action) => {
            state.loading = false;
            state.username = action.payload.username;
            state.role = action.payload.role;
            state.isConfirmed = action.payload.isConfirmed;
        })
        .addCase(register.rejected, (state, action) => {
            state.loading = false;
            state.error = (action.payload as any)?.error || 'Register failed';
        })

        .addCase(validateToken.fulfilled, (state, action) => {
            state.loading = false;
            state.username = action.payload.username;
            state.role = action.payload.role;
            state.isConfirmed = action.payload.isConfirmed;
        })

        .addCase(logout.pending, (state) => {
            state.loading = true;
            state.error = null;
        })
        .addCase(logout.fulfilled, (state) => {
            state.loading = false;
            state.username = null;
        })
        .addCase(logout.rejected, (state, action) => {
            state.loading = false;
            state.error = (action.payload as any)?.error || 'Logout failed';
        })

        .addCase(confirmEmail.pending, (state) => {
            state.loading = true;
            state.error = null;
        })
        .addCase(confirmEmail.fulfilled, (state, action) => {
            state.loading = false;
            state.isConfirmed = action.payload.isConfirmed;
        })
        .addCase(confirmEmail.rejected, (state, action) => {
            state.loading = false;
            state.error = (action.payload as any)?.error || 'Confirming failed';
        })

        .addCase(resendEmail.pending, (state) => {
            state.loading = true;
            state.error = null;
        })
        .addCase(resendEmail.fulfilled, (state, action) => {
            state.loading = false;
        })
        .addCase(resendEmail.rejected, (state, action) => {
            state.loading = false;
            state.error = (action.payload as any)?.error || 'Resending email failed';
        });
    },
});

export default authSlice.reducer;
