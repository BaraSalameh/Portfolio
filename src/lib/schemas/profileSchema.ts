import { z } from 'zod';

const phoneRegex = /^\+?[1-9]\d{1,14}$/;

export const profileSchema = z.object({
    firstname: z
        .string()
        .min(2, 'Firstname is required'),

    lastname: z
        .string()
        .min(2, 'Firstname is required'),

    title: z
        .string()
        .min(2, 'Firstname is required')
        .optional()
        .nullable(),

    bio: z
        .string()
        .max(1000, 'Bio is too long')
        .optional()
        .nullable(),

    phone: z
        .string()
        .min(9, 'Phone is short')
        .regex(phoneRegex, 'Phone must be valid'),

    profilePicture: z
        .string()
        .max(1000, 'Image string is too long')
        .optional()
        .nullable(),

    gender: z
        .string()
        .optional()
        .nullable(),

    birthDate: z.string()
        .refine(val => !isNaN(Date.parse(val)), { message: 'Birthdate not valid' }),

}).superRefine((data, ctx) => {
    const now = new Date();
    const birthDate = new Date(data.birthDate);

    if (isNaN(birthDate.getTime())) {
        ctx.addIssue({
            path: ['birthDate'],
            message: 'Birthdate not valid',
            code: z.ZodIssueCode.custom,
        });
    } else if (birthDate > now) {
        ctx.addIssue({
            path: ['birthDate'],
            message: 'Birthdate cannot be in the future',
            code: z.ZodIssueCode.custom,
        });
    }
});

export type ProfileFormData = z.infer<typeof profileSchema>;