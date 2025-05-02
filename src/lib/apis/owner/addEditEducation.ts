import { EducationFormData } from "@/lib/schemas/educationSchema";
import { transformPayload } from "@/lib/utils/appFunctions";
import { createAsyncThunk } from "@reduxjs/toolkit";

export const addEditEducation = createAsyncThunk(
    'education/addEditEducation',
    async (payload: EducationFormData, thunkAPI) => {
        try {
            const request = transformPayload(payload);
            
            const response = await fetch('https://localhost:7206/api/Owner/AddEditEducation', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(request),
                credentials: 'include'
            });

            if (!response.ok){
                const error = await response.json();
                return thunkAPI.rejectWithValue(error)
            }

        } catch (error) {
            return thunkAPI.rejectWithValue(['Unexpected error occurred']);
        }
    }
);