import { z } from 'zod';

export const educationSchema = z.object({
    institution: z.string()
        .min(2, 'Institution name must be at least 2 characters')
        .max(100, 'Institution name is too long'),

    degree: z.string()
        .min(2, 'Degree must be at least 2 characters')
        .max(100, 'Degree is too long'),

    fieldOfStudy: z.string()
        .min(2, 'Field of study must be at least 2 characters')
        .max(100, 'Field of study is too long'),

    startDate: z.string()
        .refine(val => !isNaN(Date.parse(val)), { message: 'Start date not valid' }),

    endDate: z.string()
        .refine(val => !isNaN(Date.parse(val)), { message: 'End date not valid' })
        .optional(),

    description: z.string()
        .max(1000, 'Description is too long')
        .optional(),

    isStudying: z.boolean({
        required_error: "Please specify if you are still studying",
    }),
}).refine((data) => {
    if (!data.isStudying && !data.endDate) {
        return false;
    }
    return true;
}, {
    path: ['endDate'],
    message: 'End date is required if not still studying',
});

export type EducationFormData = z.infer<typeof educationSchema>;
