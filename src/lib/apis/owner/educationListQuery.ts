import { createAsyncThunk } from '@reduxjs/toolkit';

export const educationListQuery = createAsyncThunk(
    'education/educationListQuery',
    async (_, thunkAPI)  => {
        try {
            const response = await fetch(`https://localhost:7206/api/Owner/EducationList`,
                {
                    method: 'GET',
                    headers: { 'Content-Type': 'application/json' },
                    credentials: 'include'
                }
            );
        
            if (!response.ok) {
                const error = await response.json();
                return thunkAPI.rejectWithValue(error);
            }

            if (response.status === 204){
                return [];
            }

            const data = await response.json();
            return [...data.items];

        } catch (error) {
            return thunkAPI.rejectWithValue((error as Error).message);
        }
    }
);
