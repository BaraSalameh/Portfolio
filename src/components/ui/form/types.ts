import { AnchorVariantProps } from "@/styles/anchor";
import { ButtonVariantProps } from "@/styles/button";
import React, { InputHTMLAttributes } from "react";
import { Control, FieldError, FieldPath, FieldValues, Path, UseFormRegisterReturn } from "react-hook-form";
import { ActionMeta, MultiValue, SingleValue } from "react-select";
import { z } from "zod";

type As = 'Input' | 'Checkbox' | 'Dropdown' | 'DropdownMulti' | 'Modal' | 'FieldArray';
type Type = 'Password' | 'Email' | 'Text' | 'Number' | 'Date' | 'Textarea';
type ModalAs = 'create' | 'update' | 'delete' | 'none';

export interface ControlledFormProps<T extends z.ZodTypeAny> {
    schema: T;
    onSubmit: (data: z.infer<T>) => void;
    items: {
        as: As;
        label?: string;
        type?: Type;
        placeholder?: string;
        name: Path<z.infer<T>>;
        options?: Option[];
        modal?: {
            as: ModalAs;
            children: React.ReactNode;
            title?: string;
            subTitle?: string;
        }
    }[];
    error?: string | string[] | null;
    loading?: boolean;
    className?: string;
    defaultValues?: Partial<z.infer<T>>;
    watch?: {
        name: Path<z.infer<T>>;
        defaultValue: boolean;
        watched: Path<z.infer<T>>
    };
    resetItems?: T;
    indicator?: {
        when: string;
        while?: string;
    };
    children?: React.ReactNode;
}

export interface AnchorProps extends AnchorVariantProps {
    children: React.ReactNode;
    className?: string;
    type?: 'button' | 'submit' | 'reset';
    url?: string;
}

export interface ButtonProps extends ButtonVariantProps {
    children: React.ReactNode;
    className?: string;
    type?: 'button' | 'submit' | 'reset';
    onClick?: () => void;
    onClose?: () => void;
    url?: string;
    disabled?: boolean;
}

export type Option = {
    label: string;
    value: string
}

export interface ControlledDropdownProps<T extends FieldValues>  {
    name: FieldPath<T>;
    control: Control<T>;
    label: string;
    options: Option[];
    isMulti?: boolean;
    errors?: Partial<Record<keyof T, any>>;
}

export interface FormCheckboxProps extends InputHTMLAttributes<HTMLInputElement> {
    label?: string;
    registration?: UseFormRegisterReturn;
    error?: FieldError;
}

export interface FormDropdownProps {
    label?: string;
    options: Option[];
    value?: Option | MultiValue<Option>;
    onChange?: (value: MultiValue<Option> | SingleValue<Option>, actionMeta: ActionMeta<Option>) => void;
    onBlur?: () => void;
    error?: FieldError;
    isSearchable?: boolean;
    isClearable?: boolean;
    isMulti?: boolean;
    placeholder?: string;
    isLoading?: boolean;
}

export interface FormInputProps extends InputHTMLAttributes<HTMLInputElement> {
    label?: string;
    registration?: UseFormRegisterReturn;
    error?: FieldError;
}

export interface ImageUploaderProps {
    preset: 'Profile_Picture' | 'Cover_Photo';
    onAction?: (url: string) => void;
    onClose?: () => void;
}