import { createAsyncThunk } from '@reduxjs/toolkit';

export const educationListQuery = createAsyncThunk(
    'education/educationListQuery',
    async ({username, query} : {username: string, query?: string}, thunkAPI)  => {
        try {
            const res = await fetch(`https://localhost:7206/api/Owner/EducationList?Username=${username}&Search=${query ?? ''}`,
                {
                    method: 'GET',
                    headers: { 'Content-Type': 'application/json' },
                    credentials: 'include'
                }
            );
        
            if (!res.ok) {
                const error = await res.json();
                return thunkAPI.rejectWithValue(error?.message || 'Failed to fetch education list');
            }
        
            const data = await res.json();
            return data;
        } catch (error) {
            return thunkAPI.rejectWithValue((error as Error).message);
        }
    }
);
