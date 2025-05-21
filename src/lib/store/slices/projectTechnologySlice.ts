import { createSlice } from '@reduxjs/toolkit';
import { institutionListQuery } from '@/lib/apis/owner/education/institutionListQuery';
import { degreeListQuery } from '@/lib/apis/owner/education/degreeListQuery';
import { fieldOfStudyListQuery } from '@/lib/apis/owner/education/fieldOfStudyListQuery';
import { userFullInfoQuery } from '@/lib/apis/owner/user/userFullInfoQuery';
import { educationListQuery } from '@/lib/apis/owner/education/educationListQuery';
import { addEditEducation } from '@/lib/apis/owner/education/addEditEducation';
import { deleteEducation } from '@/lib/apis/owner/education/deleteEducation';
import { EducationFormData } from '@/lib/schemas/educationSchema';
import { userByUsernameQuery } from '@/lib/apis/client/userBuUsernameQuery';
import { ProjectTechnologyFormData, TechnologyFormData } from '@/lib/schemas/projectTechnologyScehma';
import { projectTechnologyListQuery } from '@/lib/apis/owner/projectTechnology/projectTechnologyListQuery';
import { technologyListQuery } from '@/lib/apis/owner/projectTechnology/technologyListQuery';

interface ProjectTechnologyState {
    lstProjectTechnologies: ProjectTechnologyFormData[],
    lstTechnologies: TechnologyFormData[];
    loading: boolean;
    error: string | null;
}

const initialState : ProjectTechnologyState = {
    lstProjectTechnologies: [],
    lstTechnologies: [],
    loading: false,
    error: null as string | null
}

const projectTechnologySlice = createSlice({
    name: 'projectTechnology',
    initialState,
    reducers: {},
    extraReducers: (builder) => {
        builder
        .addCase(userFullInfoQuery.fulfilled, (state, action) => {
            state.lstProjectTechnologies = action.payload.lstProjects;
        })
        
        .addCase(projectTechnologyListQuery.pending, (state) => {
            state.loading = true;
            state.error = null;
        })
        .addCase(projectTechnologyListQuery.fulfilled, (state, action) => {
            state.loading = false;
            state.lstProjectTechnologies = action.payload;
        })
        .addCase(projectTechnologyListQuery.rejected, (state, action) => {
            state.loading = false;
            state.error = action.payload as string;
        })
        
        .addCase(technologyListQuery.pending, (state) => {
            state.loading = true;
            state.error = null;
        })
        .addCase(technologyListQuery.fulfilled, (state, action) => {
            state.loading = false;
            state.lstTechnologies = action.payload;
        })
        .addCase(technologyListQuery.rejected, (state, action) => {
            state.loading = false;
            state.error = action.payload as string;
        });
    },
});

export default projectTechnologySlice.reducer;
