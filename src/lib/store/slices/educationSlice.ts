import { createSlice } from '@reduxjs/toolkit';
import { addEditEducation } from '@/lib/apis/owner/addEditEducation';
import { educationListQuery } from '@/lib/apis/owner/educationListQuery';
import { EducationFormData } from '@/lib/schemas/educationSchema';
import { deleteEducation } from '@/lib/apis/owner/deleteEducation';
import { userQuery } from '@/lib/apis/owner/userQuery';
import { userByUsernameQuery } from '@/lib/apis/client/userBuUsernameQuery';

interface EducationState {
    lstEducations: EducationFormData[];
    loading: boolean;
    error: string | null;
}

const initialState : EducationState = {
    lstEducations: [],
    loading: false,
    error: null as string | null
}

const educationSlice = createSlice({
    name: 'education',
    initialState,
    reducers: {},
    extraReducers: (builder) => {
        builder
        .addCase(userQuery.fulfilled, (state, action) => {
            state.lstEducations = action.payload.lstEducations;
        })

        .addCase(educationListQuery.pending, (state) => {
            state.loading = true;
            state.error = null;
        })
        .addCase(educationListQuery.fulfilled, (state, action) => {
            state.loading = false;
            state.lstEducations = action.payload;
        })
        .addCase(educationListQuery.rejected, (state, action) => {
            state.loading = false;
            state.error = action.payload as string;
        })

        .addCase(addEditEducation.pending, (state) => {
            state.loading = true;
            state.error = null;
        })
        .addCase(addEditEducation.fulfilled, (state) => {
            state.loading = false;
        })
        .addCase(addEditEducation.rejected, (state, action) => {
            state.loading = false;
            state.error = action.payload as string;
        })

        .addCase(deleteEducation.pending, (state) => {
            state.loading = true;
            state.error = null;
        })
        .addCase(deleteEducation.fulfilled, (state, action) => {
            state.loading = false;
        })
        .addCase(deleteEducation.rejected, (state, action) => {
            state.loading = false;
            state.error = action.payload as string;
        });
    },
});

export default educationSlice.reducer;
