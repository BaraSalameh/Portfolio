import { createSlice } from '@reduxjs/toolkit';
import { addEditEducation } from '@/lib/apis/owner/addEditEducation';
import { educationListQuery } from '@/lib/apis/owner/educationListQuery';

const educationSlice = createSlice({
    name: 'education',
    initialState: {
        education: [],
        status: false,
        loading: false,
        error: null as string | null,
    },
    reducers: {},
    extraReducers: (builder) => {
        builder
        .addCase(addEditEducation.pending, (state) => {
            state.loading = true;
            state.error = null;
        })
        .addCase(addEditEducation.fulfilled, (state, action) => {
            state.loading = false;
            state.status = action.payload.status;
        })
        .addCase(addEditEducation.rejected, (state, action) => {
            state.loading = false;
            state.error = (action.payload as any)?.error || 'Education fetch failed';
        })
        
        .addCase(educationListQuery.pending, (state) => {
            state.loading = true;
            state.error = null;
        })
        .addCase(educationListQuery.fulfilled, (state, action) => {
            state.loading = false;
            state.education = action.payload.items;
        })
        .addCase(educationListQuery.rejected, (state, action) => {
            state.loading = false;
            state.error = (action.payload as any)?.error || 'get Education failed';
        });
    },
});

export default educationSlice.reducer;
