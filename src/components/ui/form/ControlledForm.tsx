'use client';

import { useForm, useWatch } from "react-hook-form";
import { ControlledFormProps } from "./types";
import { zodResolver } from "@hookform/resolvers/zod";
import { Paragraph } from "../Paragraph";
import { Button } from "./Button";
import Image from "next/image";
import { List } from "../List";
import { z } from "zod";
import { FormInput } from "./FormInput";
import { FormCheckbox } from "./FormCheckbox";
import { ControlledDropdown } from "./ControlledDropdown";
import { useEffect } from "react";
import { CUDModal } from "..";
import React from "react";

export const ControlledForm = <T extends z.ZodTypeAny> ({ 
    schema,
    onSubmit,
    items,
    error,
    loading,
    className,
    defaultValues,
    watch,
    resetItems,
    indicator,
    children
}: ControlledFormProps<T>) => {

    const {
        control,
        register,
        handleSubmit,
        reset,
        setValue,
        formState: { errors },
    } = useForm<z.infer<T>>({
        resolver: zodResolver(schema),
        defaultValues: defaultValues as z.infer<T>
    });

    const toWatch = watch
    ?   useWatch({
            control,
            name: watch.name,
            defaultValue: watch.defaultValue as any,
        })
    : null;

    useEffect(() => {
        reset(resetItems);
    }, [resetItems]);

    return (
        <form onSubmit={handleSubmit(onSubmit)} className={`relative space-y-4 ${className}`}>
            {children}
            <fieldset disabled={loading} className="space-y-4">
            {
                items.map((item, index) => {
                    if(toWatch && watch?.watched === item.name && watch.defaultValue !== toWatch) return null;

                    switch (item.as) {
                        case 'Input':
                            return (
                                <FormInput
                                    key={index}
                                    label={item.label}
                                    type={item.type || 'text'}
                                    placeholder={item.placeholder}
                                    registration={register(item.name)}
                                    error={(errors as any)[item.name]}
                                />
                            )
                        case 'Checkbox':
                            return (
                                <FormCheckbox
                                    key={index}
                                    label={item.label}
                                    registration={register(item.name)}
                                    error={(errors as any)[item.name]}
                                />
                            )
                        case 'Dropdown':
                        case 'DropdownMulti':
                            return (
                                <ControlledDropdown
                                    key={index}
                                    control={control}
                                    errors={errors}
                                    name={item.name}
                                    label={item.label || 'Select'}
                                    options={item.options || []}
                                    isMulti={item.as === 'DropdownMulti'}
                                />
                            )
                        case 'Modal':
                            return (
                                <CUDModal key={index} as={item.modal?.as} title={item.modal?.title} subTitle={item.modal?.subTitle}>
                                    {React.isValidElement(item.modal?.children)
                                        ?   React.cloneElement(item.modal?.children as React.ReactElement<{ onAction: (value: any) => void }>, {
                                                onAction: (item.modal?.children as any)?.props.onAction ?? ((value) => setValue(item.name, value))
                                            })
                                        :   item.modal?.children
                                    }
                                </CUDModal>
                            )
                        case 'FieldArray':
                        default: return null;
                    }
                })
            }
            </fieldset>
            {Array.isArray(error) && error.length > 1 ? (
                <List intent="danger" size="sm">
                    {error.map((e: string, i: number) => (
                        <li key={i}>{e}</li>
                    ))}
                </List>
            ) : (
                error && <Paragraph intent="danger" size="sm">{error}</Paragraph>
            )}

            <Button intent="standard" rounded="full" size="lg" type="submit" disabled={loading}>
                <Image
                    className="dark:invert"
                    src="/vercel.svg"
                    alt="Vercel logomark"
                    width={20}
                    height={20}
                />
                {loading ? indicator?.while || 'Submitting...' : indicator?.when || 'Submit'}
            </Button>
        </form>
    )
}