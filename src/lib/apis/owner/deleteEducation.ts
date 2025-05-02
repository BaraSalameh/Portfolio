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

            if(!response.ok){
                const error = await response.json();
                return thunkAPI.rejectWithValue(error);
            }

        } catch (error) {
            return thunkAPI.rejectWithValue(['Unexpected error occurred']);
        }
    }
);