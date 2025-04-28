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
        .optional(),

    description: z.string()
        .max(1000, 'Description is too long')
        .optional(),

    isStudying: z.boolean({
        required_error: "Please specify if you are still studying",
    }),
}).superRefine((data, ctx) => {
    // Validate endDate format if provided
    if (data.endDate && isNaN(Date.parse(data.endDate))) {
            ctx.addIssue({
            path: ['endDate'],
            message: 'End date not valid',
            code: z.ZodIssueCode.custom,
        });
    }

    // Validate presence of endDate if not studying
    if (!data.isStudying && !data.endDate) {
            ctx.addIssue({
            path: ['endDate'],
            message: 'End date is required if not still studying',
            code: z.ZodIssueCode.custom,
        });
    }
});

export type EducationFormData = z.infer<typeof educationSchema>;
