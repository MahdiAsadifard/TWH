import { DrawerProps } from "@fluentui/react-components";
import React, { useState } from "react";
import { Outlet, useNavigate, NavLink } from "react-router";
import {
  AppItem,
  Hamburger,
  NavCategory,
  NavCategoryItem,
  NavDivider,
  NavDrawer,
  NavDrawerBody,
  NavDrawerHeader,
  NavDrawerProps,
  NavItem,
  NavSectionHeader,
  NavSubItem,
  NavSubItemGroup,
} from "@fluentui/react-nav-preview";

import {
  Label,
  Radio,
  RadioGroup,
  Switch,
  Tooltip,
  makeStyles,
  tokens,
  useId,
} from "@fluentui/react-components";
import {
  Board20Filled,
  Board20Regular,
  BoxMultiple20Filled,
  BoxMultiple20Regular,
  DataArea20Filled,
  DataArea20Regular,
  DocumentBulletListMultiple20Filled,
  DocumentBulletListMultiple20Regular,
  HeartPulse20Filled,
  HeartPulse20Regular,
  MegaphoneLoud20Filled,
  MegaphoneLoud20Regular,
  NotePin20Filled,
  NotePin20Regular,
  People20Filled,
  People20Regular,
  PeopleStar20Filled,
  PeopleStar20Regular,
  Person20Filled,
  PersonLightbulb20Filled,
  PersonLightbulb20Regular,
  Person20Regular,
  PersonSearch20Filled,
  PersonSearch20Regular,
  PreviewLink20Filled,
  PreviewLink20Regular,
  bundleIcon,
  PersonCircle32Regular,
  DoorArrowLeft20Regular,
  DoorArrowLeft20Filled
} from "@fluentui/react-icons";

import { paths } from "../../Routs/Router";



const Person = bundleIcon(Person20Filled, Person20Regular);
const Dashboard = bundleIcon(Board20Filled, Board20Regular);
const Announcements = bundleIcon(MegaphoneLoud20Filled, MegaphoneLoud20Regular);
const EmployeeSpotlight = bundleIcon(PersonLightbulb20Filled, PersonLightbulb20Regular);
const Search = bundleIcon(PersonSearch20Filled, PersonSearch20Regular);
const PerformanceReviews = bundleIcon(PreviewLink20Filled, PreviewLink20Regular);
const JobPostings = bundleIcon(NotePin20Filled, NotePin20Regular);
const Interviews = bundleIcon(People20Filled, People20Regular);
const HealthPlans = bundleIcon(HeartPulse20Filled, HeartPulse20Regular);
const TrainingPrograms = bundleIcon(BoxMultiple20Filled, BoxMultiple20Regular);
const CareerDevelopment = bundleIcon(PeopleStar20Filled, PeopleStar20Regular);
const Analytics = bundleIcon(DataArea20Filled, DataArea20Regular);
const Reports = bundleIcon(DocumentBulletListMultiple20Filled, DocumentBulletListMultiple20Regular);
const Signout = bundleIcon(DoorArrowLeft20Filled, DoorArrowLeft20Regular);

type DrawerType = Required<DrawerProps>["type"];

interface IProps extends  React.PropsWithChildren {
    navProps?: Partial<NavDrawerProps>;
};

// const Nav = (props: Partial<NavDrawerProps>) => {
const Nav: React.FunctionComponent<IProps> = ({
    navProps,
}): React.ReactElement => {
  const navigate = useNavigate();
  const styles = useStyles();

  const [isOpen, setIsOpen] = useState(true);
  const [type, setType] = useState<DrawerType>("inline");
  const [isMultiple, setIsMultiple] = useState(true);

  const linkDestination = (path = paths.all) => navigate(path);

  const renderHamburgerWithToolTip = () => {
    return (
      <Tooltip content="Navigation" relationship="label">
        <Hamburger onClick={() => setIsOpen(!isOpen)} />
      </Tooltip>
    );
  };

  return (
    <div className={`${styles.root} ${!isOpen && styles.rootClosed}`}>
      <NavDrawer
        defaultSelectedValue="1"
        defaultSelectedCategoryValue=""
        open={isOpen}
        type={type}
        multiple={isMultiple}
      >
        <NavDrawerHeader>{renderHamburgerWithToolTip()}</NavDrawerHeader>

        <NavDrawerBody>
          <AppItem
            icon={<PersonCircle32Regular />}
            as="button"
            onClick={()=>{
              linkDestination(paths.profile)
            }}
          >
            Mahdi Asadifard
          </AppItem>
          <NavItem as="button" onClick={() => linkDestination(paths.dashboard)} icon={<Dashboard />} value="1">
            Dashboard
          </NavItem>
          <NavItem as="button" onClick={() => linkDestination()} icon={<Announcements />} value="2">
            Announcements
          </NavItem>
          <NavItem
            as="button"
            onClick={() => linkDestination()}
            icon={<EmployeeSpotlight />}
            value="3"
          >
            Employee Spotlight
          </NavItem>
          <NavItem icon={<Search />} as="button" onClick={() => linkDestination()}value="4">
            Profile Search
          </NavItem>
          <NavItem
            icon={<PerformanceReviews />}
            as="button" onClick={() => linkDestination()}
            value="5"
          >
            Performance Reviews
          </NavItem>
          <NavSectionHeader>Employee Management</NavSectionHeader>
          <NavCategory value="6">
            <NavCategoryItem icon={<JobPostings />}>
              Job Postings
            </NavCategoryItem>
            <NavSubItemGroup>
              <NavSubItem as="button" onClick={() => linkDestination()} value="7">
                Openings
              </NavSubItem>
              <NavSubItem as="button" onClick={() => linkDestination()} value="8">
                Submissions
              </NavSubItem>
            </NavSubItemGroup>
          </NavCategory>
          <NavItem icon={<Interviews />} value="9">
            Interviews
          </NavItem>

          <NavSectionHeader>Benefits</NavSectionHeader>
          <NavItem icon={<HealthPlans />} value="10">
            Health Plans
          </NavItem>
          <NavCategory value="11">
            <NavCategoryItem icon={<Person />} value="12">
              Retirement
            </NavCategoryItem>
            <NavSubItemGroup>
              <NavSubItem as="button" onClick={() => linkDestination()} value="13">
                Plan Information
              </NavSubItem>
              <NavSubItem as="button" onClick={() => linkDestination()} value="14">
                Fund Performance
              </NavSubItem>
            </NavSubItemGroup>
          </NavCategory>

          <NavSectionHeader>Learning</NavSectionHeader>
          <NavItem icon={<TrainingPrograms />} value="15">
            Training Programs
          </NavItem>
          <NavCategory value="16">
            <NavCategoryItem icon={<CareerDevelopment />}>
              Career Development
            </NavCategoryItem>
            <NavSubItemGroup>
              <NavSubItem as="button" onClick={() => linkDestination()} value="17">
                Career Paths
              </NavSubItem>
              <NavSubItem as="button" onClick={() => linkDestination()} value="18">
                Planning
              </NavSubItem>
            </NavSubItemGroup>
          </NavCategory>
          <NavDivider />
          <NavItem target="_blank" icon={<Analytics />} value="19">
            Workforce Data
          </NavItem>
          <NavItem as="button" onClick={() => linkDestination()} icon={<Reports />} value="20">
            Reports
          </NavItem>
          <NavItem as="button" onClick={() => linkDestination(paths.signout)} icon={<Signout />} value="21">
            Signout
          </NavItem>
        </NavDrawerBody>
      </NavDrawer>
      <div className={styles.content}>
        { !isOpen && renderHamburgerWithToolTip() }
        <Outlet />
      </div>
    </div>
  );
};

const useStyles = makeStyles({
    root: {
        overflow: "hidden",
        display: "flex",
       height: "100vh",
    },
    rootClosed: {
        //alignItems: 'flex-start',
    },
    content: {
        flex: "1",
        padding: "10px",
        //display: "grid",
        //justifyContent: "flex-start",
        //alignItems: "flex-start",
    },
    field: {
        display: "flex",
        marginTop: "4px",
        marginLeft: "8px",
        flexDirection: "column",
        gridRowGap: tokens.spacingVerticalS,
    },
});

export default Nav;