import { createAsyncThunk } from "@reduxjs/toolkit";

export const deleteEducation = createAsyncThunk(
    'education/deleteEducation',
    async (id: string, thunkAPI) => {
        try {

            const response = await fetch('https://localhost:7206/api/Owner/DeleteEducation', {
                method: 'DELETE',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({id: id}),
                credentials: 'include'
            });

            const data = await response.json();

            if(!data.status){
                return thunkAPI.rejectWithValue({
                    error: data.lstError
                });
            }
    
            return data;

        } catch (error) {
            return thunkAPI.rejectWithValue('Network error');
        }
    }
);