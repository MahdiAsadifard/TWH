import React, { ReactElement, useState } from "react";

import {
  Dialog,
  DialogTrigger,
  DialogSurface,
  DialogTitle,
  DialogBody,
  DialogActions,
  DialogContent,
  Button,
  ButtonProps
} from "@fluentui/react-components";

type ActionType = {
    title: string;
    apperance: ButtonProps["appearance"];
    onClick?: (action: ActionType) => Promise<void> | void;
};
interface IProps {
    title: string | ReactElement;
    content: string | ReactElement;
    fluid?: boolean;
    actions: Array<ActionType>,
    action?: ActionType,
    openFromOutside?: boolean;
    closeAndBackHistory?: boolean; // i.e: on signout cancel button, needs to back to the last url
};

const CustomDialog: React.FC<IProps> = ({
    title,
    content,
    fluid = true,
    actions,
    action = {
        title: '',
        apperance: 'transparent'
    },
    openFromOutside = false,
    closeAndBackHistory = false,
}): React.ReactElement => {
    const[isOpen, setIsOpen] = useState(false || openFromOutside);

    return(
        <Dialog modalType="modal" open={isOpen} onOpenChange={(event, data) => { setIsOpen(data.open) }}>
            <DialogTrigger disableButtonEnhancement>
                <Button appearance={action.apperance}>{action.title}</Button>
            </DialogTrigger>
            <DialogSurface>
                <DialogBody>
                <DialogTitle>{title}</DialogTitle>
                <DialogContent>
                    {content}
                </DialogContent>
                <DialogActions fluid={fluid}>
                    <DialogTrigger disableButtonEnhancement>
                        <Button
                            appearance="secondary"
                            className="button-close"
                            onClick={() => {
                                if(closeAndBackHistory) window.history.back();
                            }}
                        >Close</Button>
                    </DialogTrigger>
                        {
                            actions.map((_action, index) => {
                                return( 
                                    <Button 
                                        key={index}
                                        appearance={_action.apperance}
                                        onClick={() => {
                                            if(_action.onClick) _action.onClick(_action);
                                            setIsOpen(false);
                                        }}
                                    >
                                            {_action.title}
                                    </Button>
                                )
                            })
                        }
                </DialogActions>
                </DialogBody>
            </DialogSurface>
            </Dialog>
    );
};

export default CustomDialog;