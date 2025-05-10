import { z } from 'zod';

export const educationSchema = z.object({
    id: z.string().optional(),

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

    endDate: z.string().optional().nullable(),

    description: z.string()
        .max(1000, 'Description is too long')
        .optional().nullable(),

    isStudying: z.boolean().optional(),
}).superRefine((data, ctx) => {
    const now = new Date();
    const start = new Date(data.startDate);

    if (isNaN(start.getTime())) {
        ctx.addIssue({
            path: ['startDate'],
            message: 'Start date not valid',
            code: z.ZodIssueCode.custom,
        });
    } else if (start > now) {
        ctx.addIssue({
            path: ['startDate'],
            message: 'Start date cannot be in the future',
            code: z.ZodIssueCode.custom,
        });
    }

    if (!data.isStudying) {
        if (!data.endDate) {
            ctx.addIssue({
                path: ['endDate'],
                message: 'End date is required if not still studying',
                code: z.ZodIssueCode.custom,
            });
        } else {
            const end = new Date(data.endDate);
            if (isNaN(end.getTime())) {
                ctx.addIssue({
                    path: ['endDate'],
                    message: 'End date not valid',
                    code: z.ZodIssueCode.custom,
                });
            } else {
                if (end > now) {
                    ctx.addIssue({
                        path: ['endDate'],
                        message: 'End date cannot be in the future',
                        code: z.ZodIssueCode.custom,
                    });
                }
                if (start > end) {
                    ctx.addIssue({
                        path: ['endDate'],
                        message: 'End date must be after start date',
                        code: z.ZodIssueCode.custom,
                    });
                }
            }
        }
    } else {
        data.endDate = null;
    }
});

export type EducationFormData = z.infer<typeof educationSchema>;