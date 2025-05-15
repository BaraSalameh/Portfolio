import { z } from 'zod';

const guidRegex = /^[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[1-5][0-9a-fA-F]{3}-[89abAB][0-9a-fA-F]{3}-[0-9a-fA-F]{12}$/;

export const educationSchema = z.object({
    id: z.string().optional(),

    LKP_InstitutionID: z.string()
        .regex(guidRegex, 'Institution ID must be a valid GUID'),

    LKP_DegreeID: z.string()
        .regex(guidRegex, 'Degree ID must be a valid GUID'),

    LKP_FieldOfStudyID: z.string()
        .regex(guidRegex, 'Field ID must be a valid GUID'),

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