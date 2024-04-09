import * as z from "zod";

export const loginSchema = z.object({
    email: z.string({required_error: 'Email is required'}).email('Invalid email format'),
    password: z.string({required_error: 'Password is required'}).min(8, ' ')
});

export type LoginSchema = z.infer<typeof loginSchema>;


export interface LoginResponse {
    userId: string;
    token: string;
    userEmail: string;
}