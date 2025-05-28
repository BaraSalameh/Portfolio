import { createSlice } from '@reduxjs/toolkit';
import { ContactMessageFormData } from '@/lib/schemas';
import { contactMessageListQuery, deleteMessage, signMessage } from '@/lib/apis';

interface ContactMessageState {
    lstMessages: ContactMessageFormData[];
    loading: boolean;
    error: string | null;
}

const initialState : ContactMessageState = {
    lstMessages: [],
    loading: false,
    error: null as string | null
}

const contactMessageSlice = createSlice({
    name: 'contactMessage',
    initialState,
    reducers: {},
    extraReducers: (builder) => {
        builder
        .addCase(contactMessageListQuery.pending, (state) => {
            state.loading = true;
            state.error = null;
        })
        .addCase(contactMessageListQuery.fulfilled, (state, action) => {
            state.loading = false;
            state.lstMessages = action.payload;
        })
        .addCase(contactMessageListQuery.rejected, (state, action) => {
            state.loading = false;
            state.error = action.payload as string;
        })

        .addCase(signMessage.pending, (state) => {
            state.loading = true;
            state.error = null;
        })
        .addCase(signMessage.fulfilled, (state) => {
            state.loading = false;
        })
        .addCase(signMessage.rejected, (state, action) => {
            state.loading = false;
            state.error = action.payload as string;
        })

        .addCase(deleteMessage.pending, (state) => {
            state.loading = true;
            state.error = null;
        })
        .addCase(deleteMessage.fulfilled, (state) => {
            state.loading = false;
        })
        .addCase(deleteMessage.rejected, (state, action) => {
            state.loading = false;
            state.error = action.payload as string;
        });
    },
});

export default contactMessageSlice.reducer;
