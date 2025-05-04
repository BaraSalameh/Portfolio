import { createSlice } from '@reduxjs/toolkit';
import { addEditEducation } from '@/lib/apis/owner/addEditEducation';
import { educationListQuery } from '@/lib/apis/owner/educationListQuery';
import { EducationFormData } from '@/lib/schemas/educationSchema';
import { deleteEducation } from '@/lib/apis/owner/deleteEducation';
import { userQuery } from '@/lib/apis/owner/userQuery';
import { userByUsernameQuery } from '@/lib/apis/client/userBuUsernameQuery';

interface ExperienceState {
    lstExperiences: EducationFormData[];
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
        .addCase(userQuery.fulfilled, (state, action) => {
            state.lstExperiences = action.payload.lstExperiences;
        });
    },
});

export default ExperienceSlice.reducer;
