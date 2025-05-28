import { z } from 'zod';

const phoneRegex = /^\+?[1-9]\d{1,14}$/;

export const profileSchema = z.object({
    username: z
        .string()
        .nullable(),

    email: z
        .string()
        .nullable(),
        
    firstname: z
        .string()
        .min(2, 'Firstname is required'),

    lastname: z
        .string()
        .min(2, 'Lastname is required'),

    title: z
        .string()
        .min(2, 'title is required')
        .optional()
        .nullable(),

    bio: z
        .string()
        .max(1000, 'Bio is too long')
        .optional()
        .nullable(),

    phone: z
        .string()
        .optional()
        .nullable()
        .refine((val) => {
            if (!val) return true;
            return val.length >= 9;
        }, { message: 'Phone is short' })
        .refine((val) => {
            if (!val) return true;
            return phoneRegex.test(val);
        }, { message: 'Phone must be valid' }),

    profilePicture: z
        .string()
        .max(1000, 'Image string is too long')
        .optional()
        .nullable(),

    coverPhoto: z
        .string()
        .max(1000, 'Image string is too long')
        .optional()
        .nullable(),

    gender: z
        .string()
        .optional()
        .nullable(),

    birthDate: z
        .string()
        .optional()
        .nullable()
        .refine(val => {
            if (!val) return true; // allow null or undefined
            return !isNaN(Date.parse(val));
        }, {
            message: 'Birthdate not valid'
        })

}).superRefine((data, ctx) => {
    if (!data.birthDate) return; // Skip if null or undefined

    const birthDate = new Date(data.birthDate);
    const now = new Date();

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