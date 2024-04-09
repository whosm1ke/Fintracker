import * as z from "zod";

export const registerSchema = z.object({
    email: z.string()
        .email('Email is not valid'),
    userName: z.string({required_error: 'Username is required'})
        .min(3, 'Username should has at least 3 characters')
        .max(20, 'Username should has at most 20 characters'),
    password: z.string()
        .min(8, 'Password should has at least 8 characters'),
    confirmPassword: z.string()
        .min(8, 'Confirm password should has at least 8 characters')
}).refine(data => data.password === data.confirmPassword, {
    message: 'Passwords must match',
    path: ['confirmPassword']
});

export type RegisterSchema = z.infer<typeof registerSchema>;

export interface RegisterResponse {
    userId: string
}
