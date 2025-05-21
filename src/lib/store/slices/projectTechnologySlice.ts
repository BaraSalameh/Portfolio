import { createSlice } from '@reduxjs/toolkit';
import { userFullInfoQuery } from '@/lib/apis/owner/user/userFullInfoQuery';
import { ProjectTechnologyFormData, TechnologyFormData } from '@/lib/schemas/projectTechnologyScehma';
import { projectTechnologyListQuery } from '@/lib/apis/owner/projectTechnology/projectTechnologyListQuery';
import { technologyListQuery } from '@/lib/apis/owner/projectTechnology/technologyListQuery';
import { addEditDeleteProjectTechnology } from '@/lib/apis/owner/projectTechnology/addEdetDeleteProjectTechnology';

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
        })
        
        .addCase(addEditDeleteProjectTechnology.pending, (state) => {
            state.loading = true;
            state.error = null;
        })
        .addCase(addEditDeleteProjectTechnology.fulfilled, (state) => {
            state.loading = false;
        })
        .addCase(addEditDeleteProjectTechnology.rejected, (state, action) => {
            state.loading = false;
            state.error = action.payload as string;
        });
    },
});

export default projectTechnologySlice.reducer;
