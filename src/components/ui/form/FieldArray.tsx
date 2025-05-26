import { useFieldArray, Controller } from "react-hook-form";
import { ControlledDropdown } from "./ControlledDropdown";
import { Button } from "./Button";

export const FieldArray = ({
    name,
    label,
    control,
    errors,
    register,
    fields: fieldConfigs
}: any) => {
    const { fields, append, remove } = useFieldArray({
        control,
        name,
    });

    return (
        <div className="space-y-2">
            <h3 className="text-white text-sm font-medium">{label}</h3>
            <div className="space-y-3">
                {fields.map((field, i) => (
                    <div key={field.id} className="border p-5 rounded-2xl space-y-3">
                        {fieldConfigs.map((config: any, j: number) => (
                            <ControlledDropdown
                                key={j}
                                name={`${name}[${i}].${config.name}`}
                                control={control}
                                errors={errors?.[name]?.[i]?.[config.name]}
                                label={config.label}
                                options={config.options || []}
                            />
                        ))}
                        <Button
                            type="button"
                            intent="danger"
                            size="sm"
                            onClick={() => remove(i)}
                        >
                            Remove
                        </Button>
                    </div>
                ))}
            </div>
            <Button
                type="button"
                intent="standard"
                onClick={() =>
                    append({ lkP_LanguageID: '', lkP_LanguageProficiencyID: '' })
                }
            >
                Add Language
            </Button>
        </div>
    );
};
