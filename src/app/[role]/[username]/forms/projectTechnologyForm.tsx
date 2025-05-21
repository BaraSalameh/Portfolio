'use client';

import { useAppSelector, useAppDispatch } from "@/lib/store/hooks";
import { Paragraph } from "@/components/ui/Paragraph";
import { Button } from "@/components/ui/Button";
import Image from "next/image";
import { Controller, useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { FormInput } from "@/components/ui/FormInput";
import { List } from "@/components/ui/List";
import { FormCheckbox } from "@/components/ui/FormCheckbox";
import { useEffect, useMemo } from "react";
import { ProjectTechnologyProps } from "../types";
import { ProjectTechnologyFormData, projectTechnologySchema } from "@/lib/schemas/projectTechnologyScehma";
import { projectTechnologyListQuery } from "@/lib/apis/owner/projectTechnology/projectTechnologyListQuery";
import { FormDropdown } from "@/components/ui/FormDropdown";
import { getSelectedOption } from "@/lib/utils/appFunctions";
import { technologyListQuery } from "@/lib/apis/owner/projectTechnology/technologyListQuery";
import { MultiValue } from "react-select";
import { Option } from "@/components/types";
import { addEditDeleteProjectTechnology } from "@/lib/apis/owner/projectTechnology/addEdetDeleteProjectTechnology";

const ProjectTechnology = ({id, onClose} : ProjectTechnologyProps) => {

    const dispatch = useAppDispatch();
    const { loading, error, lstProjectTechnologies, lstTechnologies } = useAppSelector((state) => state.projectTechnology);
    const projectTechnologyToHandle = lstProjectTechnologies.find(pt => pt.id === id);

    const technologyOptions = useMemo(() =>
            lstTechnologies.map(i => ({ label: i.name, value: i.id }))
        , [lstTechnologies]);

    const {
        register,
        handleSubmit, 
        control,
        reset,
        formState: { errors },
    } = useForm<ProjectTechnologyFormData>({
        resolver: zodResolver(projectTechnologySchema),
        defaultValues: {
            isFeatured: false,
        }
    });

    const onSubmit = async (data: ProjectTechnologyFormData) => {
        await dispatch(addEditDeleteProjectTechnology(data));
        await dispatch(projectTechnologyListQuery());
        onClose?.();
    };

    useEffect(() => {
        if (projectTechnologyToHandle) {
            reset({...projectTechnologyToHandle});
        }
    }, [id]);

    useEffect(() => {
        lstTechnologies.length === 0 && dispatch(technologyListQuery());
    }, []);

    const ControlledDropdown = ({
        name,
        label,
        options,
    }: {
        name: keyof ProjectTechnologyFormData;
        label: string;
        options: { label: string; value: string }[];
    }) => (
        <Controller
            name={name}
            control={control}
            render={({ field }) => (
            <FormDropdown
                label={label}
                options={options}
                value={getSelectedOption(options, field.value as string)}
                onChange={(option) => {
                    const selectedIds = (option as MultiValue<Option>)?.map((opt) => opt.value) ?? [];
                    field.onChange(selectedIds);
                }}
                onBlur={field.onBlur}
                error={Array.isArray(errors[name]) 
                    ? errors[name][0] 
                    : errors[name]
                }
                isLoading={options.length === 0}
                isMulti
            />
            )}
        />
    );

    return (
        <form onSubmit={handleSubmit(onSubmit)} className="relative space-y-4">
            <fieldset disabled={loading} className="space-y-4">
                <ControlledDropdown name="lstProjectTechnologies" label="Technologies" options={technologyOptions} />
                <FormInput
                    label="Title"
                    type="text"
                    placeholder="e.g. Portfolio"
                    registration={register('title')}
                    error={errors.title}
                />

                <FormInput
                    label="Live link"
                    type="text"
                    placeholder="e.g. https://MyProject"
                    registration={register('liveLink')}
                    error={errors.liveLink}
                />

                <FormInput
                    label="Source code"
                    type="text"
                    placeholder="e.g. https://LinkedIn"
                    registration={register('sourceCode')}
                    error={errors.sourceCode}
                />

                <FormInput
                    label="Image URL"
                    type="text"
                    placeholder="e.g. https://Image"
                    registration={register('imageUrl')}
                    error={errors.imageUrl}
                />

                <FormCheckbox
                    label="Is featured"
                    registration={register('isFeatured')}
                    error={errors.isFeatured}
                />

                <FormInput
                    label="Description"
                    type="textarea"
                    placeholder="Description"
                    registration={register('description')}
                    error={errors.description}
                />
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
                {loading ? 'Submitting...' : 'Submit'}
            </Button>
        </form>
    );
}

export default ProjectTechnology;