import { addEditExperience } from '@/lib/apis/owner/experience/addEditExperience';
import { deleteExperience } from '@/lib/apis/owner/experience/deleteExperience';
import { experienceListQuery } from '@/lib/apis/owner/experience/experienceListQuery';
import { userFullInfoQuery } from '@/lib/apis/owner/user/userFullInfoQuery';
import { ExperienceFormData } from '@/lib/schemas/experienceSchema';
import { createSlice } from '@reduxjs/toolkit';

interface ExperienceState {
    lstExperiences: ExperienceFormData[];
    loading: boolean;
    error: string | null;
}

const initialState : ExperienceState = {
    lstExperiences: [],
    loading: false,
    error: null as string | null
}

const ExperienceSlice = createSlice({
    name: 'experience',
    initialState,
    reducers: {},
    extraReducers: (builder) => {
        builder
        .addCase(userFullInfoQuery.fulfilled, (state, action) => {
            state.lstExperiences = action.payload.lstExperiences;
        })
        
        .addCase(experienceListQuery.pending, (state) => {
            state.loading = true;
            state.error = null;
        })
        .addCase(experienceListQuery.fulfilled, (state, action) => {
            state.loading = false;
            state.lstExperiences = action.payload;
        })
        .addCase(experienceListQuery.rejected, (state, action) => {
            state.loading = false;
            state.error = action.payload as string;
        })
        
        .addCase(addEditExperience.pending, (state) => {
            state.loading = true;
            state.error = null;
        })
        .addCase(addEditExperience.fulfilled, (state) => {
            state.loading = false;
        })
        .addCase(addEditExperience.rejected, (state, action) => {
            state.loading = false;
            state.error = action.payload as string;
        })

        .addCase(deleteExperience.pending, (state) => {
            state.loading = true;
            state.error = null;
        })
        .addCase(deleteExperience.fulfilled, (state) => {
            state.loading = false;
        })
        .addCase(deleteExperience.rejected, (state, action) => {
            state.loading = false;
            state.error = action.payload as string;
        });
    },
});

export default ExperienceSlice.reducer;
