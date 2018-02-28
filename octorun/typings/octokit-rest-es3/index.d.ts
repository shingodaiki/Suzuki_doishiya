/**
 * This declaration file requires TypeScript 2.1 or above.
 */
declare namespace Github {
  type json = any
  type date = string

  export interface AnyResponse {
    /** This is the data you would see in https://developer.github.com/v3/ */
    data: any

    /** Request metadata */
    meta:{
      'x-ratelimit-limit': string,
      'x-ratelimit-remaining': string,
      'x-ratelimit-reset': string,
      'x-github-request-id': string,
      'x-github-media-type': string,
      link: string,
      'last-modified': string,
      etag: string,
      status: string
    }

    [Symbol.iterator](): Iterator<any>
  }

  export interface EmptyParams {
  }

  export interface Options {
    timeout?: number;
    host?: string;
    pathPrefix?: string;
    protocol?: string;
    port?: number;
    proxy?: string;
    ca?: string;
    headers?: {[header: string]: any};
    requestMedia?: string;
    rejectUnauthorized?: boolean;
    family?: number;
  }

  export interface AuthBasic  {
    type: "basic";
    username: string;
    password: string;
  }

  export interface AuthOAuthToken  {
    type: "oauth";
    token: string;
  }

  export interface AuthOAuthSecret  {
    type: "oauth";
    key: string;
    secret: string;
  }

  export interface AuthUserToken  {
    type: "token";
    token: string;
  }

  export interface AuthJWT  {
    type: "integration";
    token: string;
  }

  export type Auth =
    | AuthBasic
    | AuthOAuthToken
    | AuthOAuthSecret
    | AuthUserToken
    | AuthJWT;

  export type Link =
    | { link: string; }
    | { meta: { link: string; }; }
    | string;

  export interface Callback {
    (error: Error | null, result: any): any;
  }


  export type AuthorizationGetParams =
    & {
      id: string;
    };
  export type AuthorizationCreateParams =
    & {
      scopes?: string[];
      note?: string;
      note_url?: string;
      client_id?: string;
      client_secret?: string;
      fingerprint?: string;
    };
  export type AuthorizationUpdateParams =
    & {
      id: string;
      scopes?: string[];
      add_scopes?: string[];
      remove_scopes?: string[];
      note?: string;
      note_url?: string;
      fingerprint?: string;
    };
  export type AuthorizationDeleteParams =
    & {
      id: string;
    };
  export type AuthorizationCheckParams =
    & {
      client_id?: string;
      access_token: string;
    };
  export type AuthorizationResetParams =
    & {
      client_id?: string;
      access_token: string;
    };
  export type AuthorizationRevokeParams =
    & {
      client_id?: string;
      access_token: string;
    };
  export type AuthorizationGetGrantsParams =
    & {
      page?: number;
      per_page?: number;
    };
  export type AuthorizationGetGrantParams =
    & {
      id: string;
      page?: number;
      per_page?: number;
    };
  export type AuthorizationDeleteGrantParams =
    & {
      id: string;
    };
  export type AuthorizationGetAllParams =
    & {
      page?: number;
      per_page?: number;
    };
  export type AuthorizationGetOrCreateAuthorizationForAppParams =
    & {
      client_id?: string;
      client_secret: string;
      scopes?: string[];
      note?: string;
      note_url?: string;
      fingerprint?: string;
    };
  export type AuthorizationGetOrCreateAuthorizationForAppAndFingerprintParams =
    & {
      client_id?: string;
      fingerprint?: string;
      client_secret: string;
      scopes?: string[];
      note?: string;
      note_url?: string;
    };
  export type AuthorizationRevokeGrantParams =
    & {
      client_id?: string;
      access_token: string;
    };
  export type ActivityGetEventsParams =
    & {
      page?: number;
      per_page?: number;
    };
  export type ActivityGetEventsForRepoParams =
    & {
      owner: string;
      repo: string;
      page?: number;
      per_page?: number;
    };
  export type ActivityGetEventsForRepoIssuesParams =
    & {
      owner: string;
      repo: string;
      page?: number;
      per_page?: number;
    };
  export type ActivityGetEventsForRepoNetworkParams =
    & {
      owner: string;
      repo: string;
      page?: number;
      per_page?: number;
    };
  export type ActivityGetEventsForOrgParams =
    & {
      org: string;
      page?: number;
      per_page?: number;
    };
  export type ActivityGetEventsReceivedParams =
    & {
      username: string;
      page?: number;
      per_page?: number;
    };
  export type ActivityGetEventsReceivedPublicParams =
    & {
      username: string;
      page?: number;
      per_page?: number;
    };
  export type ActivityGetEventsForUserParams =
    & {
      username: string;
      page?: number;
      per_page?: number;
    };
  export type ActivityGetEventsForUserPublicParams =
    & {
      username: string;
      page?: number;
      per_page?: number;
    };
  export type ActivityGetEventsForUserOrgParams =
    & {
      username: string;
      org: string;
      page?: number;
      per_page?: number;
    };
  export type ActivityGetNotificationsParams =
    & {
      all?: boolean;
      participating?: boolean;
      since?: date;
      before?: string;
    };
  export type ActivityGetNotificationsForUserParams =
    & {
      owner: string;
      repo: string;
      all?: boolean;
      participating?: boolean;
      since?: date;
      before?: string;
    };
  export type ActivityMarkNotificationsAsReadParams =
    & {
      last_read_at?: string;
    };
  export type ActivityMarkNotificationsAsReadForRepoParams =
    & {
      owner: string;
      repo: string;
      last_read_at?: string;
    };
  export type ActivityGetNotificationThreadParams =
    & {
      id: string;
    };
  export type ActivityMarkNotificationThreadAsReadParams =
    & {
      id: string;
    };
  export type ActivityCheckNotificationThreadSubscriptionParams =
    & {
      id: string;
    };
  export type ActivitySetNotificationThreadSubscriptionParams =
    & {
      id: string;
      subscribed?: boolean;
      ignored?: boolean;
    };
  export type ActivityDeleteNotificationThreadSubscriptionParams =
    & {
      id: string;
    };
  export type ActivityGetStargazersForRepoParams =
    & {
      owner: string;
      repo: string;
      page?: number;
      per_page?: number;
    };
  export type ActivityGetStarredReposForUserParams =
    & {
      username: string;
      sort?: "created"|"updated";
      direction?: "asc"|"desc";
      page?: number;
      per_page?: number;
    };
  export type ActivityGetStarredReposParams =
    & {
      sort?: "created"|"updated";
      direction?: "asc"|"desc";
      page?: number;
      per_page?: number;
    };
  export type ActivityCheckStarringRepoParams =
    & {
      owner: string;
      repo: string;
      page?: number;
      per_page?: number;
    };
  export type ActivityStarRepoParams =
    & {
      owner: string;
      repo: string;
    };
  export type ActivityUnstarRepoParams =
    & {
      owner: string;
      repo: string;
    };
  export type ActivityGetWatchersForRepoParams =
    & {
      owner: string;
      repo: string;
      page?: number;
      per_page?: number;
    };
  export type ActivityGetWatchedReposForUserParams =
    & {
      username: string;
      page?: number;
      per_page?: number;
    };
  export type ActivityGetWatchedReposParams =
    & {
      page?: number;
      per_page?: number;
    };
  export type ActivityGetRepoSubscriptionParams =
    & {
      owner: string;
      repo: string;
      page?: number;
      per_page?: number;
    };
  export type ActivitySetRepoSubscriptionParams =
    & {
      owner: string;
      repo: string;
      subscribed?: boolean;
      ignored?: boolean;
    };
  export type ActivityUnwatchRepoParams =
    & {
      owner: string;
      repo: string;
    };
  export type GistsGetParams =
    & {
      id: string;
    };
  export type GistsCreateParams =
    & {
      files: json;
      description?: string;
      public: boolean;
    };
  export type GistsEditParams =
    & {
      id: string;
      description?: string;
      files: json;
      content?: string;
      filename?: string;
    };
  export type GistsStarParams =
    & {
      id: string;
    };
  export type GistsUnstarParams =
    & {
      id: string;
    };
  export type GistsForkParams =
    & {
      id: string;
    };
  export type GistsDeleteParams =
    & {
      id: string;
    };
  export type GistsGetForUserParams =
    & {
      username: string;
      since?: date;
      page?: number;
      per_page?: number;
    };
  export type GistsGetAllParams =
    & {
      since?: date;
      page?: number;
      per_page?: number;
    };
  export type GistsGetPublicParams =
    & {
      since?: date;
    };
  export type GistsGetStarredParams =
    & {
      since?: date;
    };
  export type GistsGetRevisionParams =
    & {
      id: string;
      sha: string;
    };
  export type GistsGetCommitsParams =
    & {
      id: string;
    };
  export type GistsCheckStarParams =
    & {
      id: string;
    };
  export type GistsGetForksParams =
    & {
      id: string;
      page?: number;
      per_page?: number;
    };
  export type GistsGetCommentsParams =
    & {
      gist_id: string;
    };
  export type GistsGetCommentParams =
    & {
      gist_id: string;
      id: string;
    };
  export type GistsCreateCommentParams =
    & {
      gist_id: string;
      body: string;
    };
  export type GistsEditCommentParams =
    & {
      gist_id: string;
      id: string;
      body: string;
    };
  export type GistsDeleteCommentParams =
    & {
      gist_id: string;
      id: string;
    };
  export type GitdataGetBlobParams =
    & {
      owner: string;
      repo: string;
      sha: string;
      page?: number;
      per_page?: number;
    };
  export type GitdataCreateBlobParams =
    & {
      owner: string;
      repo: string;
      content: string;
      encoding: string;
    };
  export type GitdataGetCommitParams =
    & {
      owner: string;
      repo: string;
      sha: string;
    };
  export type GitdataCreateCommitParams =
    & {
      owner: string;
      repo: string;
      message: string;
      tree: string;
      parents: string[];
      author?: json;
      committer?: json;
    };
  export type GitdataGetCommitSignatureVerificationParams =
    & {
      owner: string;
      repo: string;
      sha: string;
    };
  export type GitdataGetReferenceParams =
    & {
      owner: string;
      repo: string;
      ref: string;
    };
  export type GitdataGetReferencesParams =
    & {
      owner: string;
      repo: string;
      page?: number;
      per_page?: number;
    };
  export type GitdataGetTagsParams =
    & {
      owner: string;
      repo: string;
      page?: number;
      per_page?: number;
    };
  export type GitdataCreateReferenceParams =
    & {
      owner: string;
      repo: string;
      ref: string;
      sha: string;
    };
  export type GitdataUpdateReferenceParams =
    & {
      owner: string;
      repo: string;
      ref: string;
      sha: string;
      force?: boolean;
    };
  export type GitdataDeleteReferenceParams =
    & {
      owner: string;
      repo: string;
      ref: string;
    };
  export type GitdataGetTagParams =
    & {
      owner: string;
      repo: string;
      sha: string;
    };
  export type GitdataCreateTagParams =
    & {
      owner: string;
      repo: string;
      tag: string;
      message: string;
      object: string;
      type: string;
      tagger: json;
    };
  export type GitdataGetTagSignatureVerificationParams =
    & {
      owner: string;
      repo: string;
      sha: string;
    };
  export type GitdataGetTreeParams =
    & {
      owner: string;
      repo: string;
      sha: string;
      recursive?: boolean;
    };
  export type GitdataCreateTreeParams =
    & {
      owner: string;
      repo: string;
      tree: json;
      base_tree?: string;
    };
  export type IntegrationsGetInstallationsParams =
    & {
      page?: number;
      per_page?: number;
    };
  export type IntegrationsCreateInstallationTokenParams =
    & {
      installation_id: string;
      user_id?: string;
    };
  export type IntegrationsGetInstallationRepositoriesParams =
    & {
      user_id?: string;
    };
  export type IntegrationsAddRepoToInstallationParams =
    & {
      installation_id: string;
      repository_id: string;
    };
  export type IntegrationsRemoveRepoFromInstallationParams =
    & {
      installation_id: string;
      repository_id: string;
    };
  export type AppsGetForSlugParams =
    & {
      app_slug: string;
    };
  export type AppsGetInstallationsParams =
    & {
      page?: number;
      per_page?: number;
    };
  export type AppsGetInstallationParams =
    & {
      installation_id: string;
    };
  export type AppsCreateInstallationTokenParams =
    & {
      installation_id: string;
      user_id?: string;
    };
  export type AppsGetInstallationRepositoriesParams =
    & {
      user_id?: string;
    };
  export type AppsAddRepoToInstallationParams =
    & {
      installation_id: string;
      repository_id: string;
    };
  export type AppsRemoveRepoFromInstallationParams =
    & {
      installation_id: string;
      repository_id: string;
    };
  export type AppsGetMarketplaceListingPlansParams =
    & {
      page?: number;
      per_page?: number;
    };
  export type AppsGetMarketplaceListingStubbedPlansParams =
    & {
      page?: number;
      per_page?: number;
    };
  export type AppsGetMarketplaceListingPlanAccountsParams =
    & {
      id: string;
      page?: number;
      per_page?: number;
    };
  export type AppsGetMarketplaceListingStubbedPlanAccountsParams =
    & {
      id: string;
      page?: number;
      per_page?: number;
    };
  export type AppsCheckMarketplaceListingAccountParams =
    & {
      id: string;
    };
  export type AppsCheckMarketplaceListingStubbedAccountParams =
    & {
      id: string;
    };
  export type IssuesGetParams =
    & {
      owner: string;
      repo: string;
      number: number;
    };
  export type IssuesCreateParams =
    & {
      owner: string;
      repo: string;
      title: string;
      body?: string;
      assignee?: string;
      milestone?: number;
      labels?: string[];
      assignees?: string[];
    };
  export type IssuesEditParams =
    & {
      owner: string;
      repo: string;
      number: number;
      title?: string;
      body?: string;
      assignee?: string;
      state?: "open"|"closed";
      milestone?: number;
      labels?: string[];
      assignees?: string[];
    };
  export type IssuesLockParams =
    & {
      owner: string;
      repo: string;
      number: number;
    };
  export type IssuesUnlockParams =
    & {
      owner: string;
      repo: string;
      number: number;
    };
  export type IssuesGetAllParams =
    & {
      filter?: "all"|"assigned"|"created"|"mentioned"|"subscribed";
      state?: "open"|"closed"|"all";
      labels?: string;
      sort?: "created"|"updated"|"comments";
      direction?: "asc"|"desc";
      since?: date;
      page?: number;
      per_page?: number;
    };
  export type IssuesGetForUserParams =
    & {
      filter?: "all"|"assigned"|"created"|"mentioned"|"subscribed";
      state?: "open"|"closed"|"all";
      labels?: string;
      sort?: "created"|"updated"|"comments";
      direction?: "asc"|"desc";
      since?: date;
      page?: number;
      per_page?: number;
    };
  export type IssuesGetForOrgParams =
    & {
      org: string;
      filter?: "all"|"assigned"|"created"|"mentioned"|"subscribed";
      state?: "open"|"closed"|"all";
      labels?: string;
      sort?: "created"|"updated"|"comments";
      direction?: "asc"|"desc";
      since?: date;
      page?: number;
      per_page?: number;
    };
  export type IssuesGetForRepoParams =
    & {
      owner: string;
      repo: string;
      milestone?: string;
      state?: "open"|"closed"|"all";
      assignee?: string;
      creator?: string;
      mentioned?: string;
      labels?: string;
      sort?: "created"|"updated"|"comments";
      direction?: "asc"|"desc";
      since?: date;
      page?: number;
      per_page?: number;
    };
  export type IssuesGetAssigneesParams =
    & {
      owner: string;
      repo: string;
    };
  export type IssuesCheckAssigneeParams =
    & {
      owner: string;
      repo: string;
      assignee: string;
    };
  export type IssuesAddAssigneesToIssueParams =
    & {
      owner: string;
      repo: string;
      number: number;
      assignees: string[];
    };
  export type IssuesRemoveAssigneesFromIssueParams =
    & {
      owner: string;
      repo: string;
      number: number;
      body: json;
    };
  export type IssuesGetCommentsParams =
    & {
      owner: string;
      repo: string;
      number: number;
      since?: date;
      page?: number;
      per_page?: number;
    };
  export type IssuesGetCommentsForRepoParams =
    & {
      owner: string;
      repo: string;
      sort?: "created"|"updated";
      direction?: "asc"|"desc";
      since?: date;
      page?: number;
      per_page?: number;
    };
  export type IssuesGetCommentParams =
    & {
      owner: string;
      repo: string;
      id: string;
    };
  export type IssuesCreateCommentParams =
    & {
      owner: string;
      repo: string;
      number: number;
      body: string;
    };
  export type IssuesEditCommentParams =
    & {
      owner: string;
      repo: string;
      id: string;
      body: string;
    };
  export type IssuesDeleteCommentParams =
    & {
      owner: string;
      repo: string;
      id: string;
    };
  export type IssuesGetEventsParams =
    & {
      owner: string;
      repo: string;
      issue_number: number;
      page?: number;
      per_page?: number;
    };
  export type IssuesGetEventsForRepoParams =
    & {
      owner: string;
      repo: string;
      page?: number;
      per_page?: number;
    };
  export type IssuesGetEventParams =
    & {
      owner: string;
      repo: string;
      id: string;
    };
  export type IssuesGetLabelsParams =
    & {
      owner: string;
      repo: string;
      page?: number;
      per_page?: number;
    };
  export type IssuesGetLabelParams =
    & {
      owner: string;
      repo: string;
      name: string;
    };
  export type IssuesCreateLabelParams =
    & {
      owner: string;
      repo: string;
      name: string;
      color: string;
    };
  export type IssuesUpdateLabelParams =
    & {
      owner: string;
      repo: string;
      oldname: string;
      name: string;
      color: string;
    };
  export type IssuesDeleteLabelParams =
    & {
      owner: string;
      repo: string;
      name: string;
    };
  export type IssuesGetIssueLabelsParams =
    & {
      owner: string;
      repo: string;
      number: number;
    };
  export type IssuesAddLabelsParams =
    & {
      owner: string;
      repo: string;
      number: number;
      labels: string[];
    };
  export type IssuesRemoveLabelParams =
    & {
      owner: string;
      repo: string;
      number: number;
      name: string;
    };
  export type IssuesReplaceAllLabelsParams =
    & {
      owner: string;
      repo: string;
      number: number;
      labels: string[];
    };
  export type IssuesRemoveAllLabelsParams =
    & {
      owner: string;
      repo: string;
      number: number;
    };
  export type IssuesGetMilestoneLabelsParams =
    & {
      owner: string;
      repo: string;
      number: number;
    };
  export type IssuesGetMilestonesParams =
    & {
      owner: string;
      repo: string;
      state?: "open"|"closed"|"all";
      sort?: "due_on"|"completeness";
      direction?: "asc"|"desc";
      page?: number;
      per_page?: number;
    };
  export type IssuesGetMilestoneParams =
    & {
      owner: string;
      repo: string;
      number: number;
    };
  export type IssuesCreateMilestoneParams =
    & {
      owner: string;
      repo: string;
      title: string;
      state?: "open"|"closed"|"all";
      description?: string;
      due_on?: date;
    };
  export type IssuesUpdateMilestoneParams =
    & {
      owner: string;
      repo: string;
      number: number;
      title: string;
      state?: "open"|"closed"|"all";
      description?: string;
      due_on?: date;
    };
  export type IssuesDeleteMilestoneParams =
    & {
      owner: string;
      repo: string;
      number: number;
    };
  export type IssuesGetEventsTimelineParams =
    & {
      owner: string;
      repo: string;
      issue_number: number;
      page?: number;
      per_page?: number;
    };
  export type MigrationsStartMigrationParams =
    & {
      org: string;
      repositories: string[];
      lock_repositories?: boolean;
      exclude_attachments?: boolean;
    };
  export type MigrationsGetMigrationsParams =
    & {
      org: string;
      page?: number;
      per_page?: number;
    };
  export type MigrationsGetMigrationStatusParams =
    & {
      org: string;
      id: string;
    };
  export type MigrationsGetMigrationArchiveLinkParams =
    & {
      org: string;
      id: string;
    };
  export type MigrationsDeleteMigrationArchiveParams =
    & {
      org: string;
      id: string;
    };
  export type MigrationsUnlockRepoLockedForMigrationParams =
    & {
      org: string;
      id: string;
      repo_name: string;
    };
  export type MigrationsStartImportParams =
    & {
      owner: string;
      repo: string;
      vcs_url: string;
      vcs?: "subversion"|"git"|"mercurial"|"tfvc";
      vcs_username?: string;
      vcs_password?: string;
      tfvc_project?: string;
    };
  export type MigrationsGetImportProgressParams =
    & {
      owner: string;
      repo: string;
    };
  export type MigrationsUpdateImportParams =
    & {
      owner: string;
      repo: string;
      vcs_username?: string;
      vcs_password?: string;
    };
  export type MigrationsGetImportCommitAuthorsParams =
    & {
      owner: string;
      repo: string;
      since?: string;
    };
  export type MigrationsMapImportCommitAuthorParams =
    & {
      owner: string;
      repo: string;
      author_id: string;
      email?: string;
      name?: string;
    };
  export type MigrationsSetImportLfsPreferenceParams =
    & {
      owner: string;
      name: string;
      use_lfs: string;
    };
  export type MigrationsGetLargeImportFilesParams =
    & {
      owner: string;
      name: string;
    };
  export type MigrationsCancelImportParams =
    & {
      owner: string;
      repo: string;
    };
  export type MiscGetCodeOfConductParams =
    & {
      key: string;
    };
  export type MiscGetRepoCodeOfConductParams =
    & {
      owner: string;
      repo: string;
    };
  export type MiscGetGitignoreTemplateParams =
    & {
      name: string;
    };
  export type MiscGetLicenseParams =
    & {
      license: string;
    };
  export type MiscGetRepoLicenseParams =
    & {
      owner: string;
      repo: string;
    };
  export type MiscRenderMarkdownParams =
    & {
      text: string;
      mode?: "markdown"|"gfm";
      context?: string;
    };
  export type MiscRenderMarkdownRawParams =
    & {
      data: string;
    };
  export type OrgsGetParams =
    & {
      org: string;
      page?: number;
      per_page?: number;
    };
  export type OrgsUpdateParams =
    & {
      org: string;
      billing_email?: string;
      company?: string;
      email?: string;
      location?: string;
      name?: string;
      description?: string;
      default_repository_permission?: "read"|"write"|"admin"|"none";
      members_can_create_repositories?: boolean;
    };
  export type OrgsGetAllParams =
    & {
      since?: string;
      page?: number;
      per_page?: number;
    };
  export type OrgsGetForUserParams =
    & {
      username: string;
      page?: number;
      per_page?: number;
    };
  export type OrgsGetMembersParams =
    & {
      org: string;
      filter?: "all"|"2fa_disabled";
      role?: "all"|"admin"|"member";
      page?: number;
      per_page?: number;
    };
  export type OrgsCheckMembershipParams =
    & {
      org: string;
      username: string;
    };
  export type OrgsRemoveMemberParams =
    & {
      org: string;
      username: string;
    };
  export type OrgsGetPublicMembersParams =
    & {
      org: string;
    };
  export type OrgsCheckPublicMembershipParams =
    & {
      org: string;
      username: string;
    };
  export type OrgsPublicizeMembershipParams =
    & {
      org: string;
      username: string;
    };
  export type OrgsConcealMembershipParams =
    & {
      org: string;
      username: string;
    };
  export type OrgsGetOrgMembershipParams =
    & {
      org: string;
      username: string;
    };
  export type OrgsAddOrgMembershipParams =
    & {
      org: string;
      username: string;
      role: "admin"|"member";
    };
  export type OrgsRemoveOrgMembershipParams =
    & {
      org: string;
      username: string;
    };
  export type OrgsGetPendingOrgInvitesParams =
    & {
      org: string;
    };
  export type OrgsGetOutsideCollaboratorsParams =
    & {
      org: string;
      filter?: "all"|"2fa_disabled";
      page?: number;
      per_page?: number;
    };
  export type OrgsRemoveOutsideCollaboratorParams =
    & {
      org: string;
      username: string;
    };
  export type OrgsConvertMemberToOutsideCollaboratorParams =
    & {
      org: string;
      username: string;
    };
  export type OrgsGetTeamsParams =
    & {
      org: string;
      page?: number;
      per_page?: number;
    };
  export type OrgsGetTeamParams =
    & {
      id: string;
    };
  export type OrgsCreateTeamParams =
    & {
      org: string;
      name: string;
      description?: string;
      maintainers?: string[];
      repo_names?: string[];
      privacy?: "secret"|"closed";
      parent_team_id?: string;
    };
  export type OrgsEditTeamParams =
    & {
      id: string;
      name: string;
      description?: string;
      privacy?: "secret"|"closed";
      parent_team_id?: string;
    };
  export type OrgsDeleteTeamParams =
    & {
      id: string;
    };
  export type OrgsGetTeamMembersParams =
    & {
      id: string;
      role?: "member"|"maintainer"|"all";
      page?: number;
      per_page?: number;
    };
  export type OrgsGetChildTeamsParams =
    & {
      id: string;
      page?: number;
      per_page?: number;
    };
  export type OrgsGetTeamMembershipParams =
    & {
      id: string;
      username: string;
    };
  export type OrgsAddTeamMembershipParams =
    & {
      id: string;
      username: string;
      role?: "member"|"maintainer";
    };
  export type OrgsRemoveTeamMembershipParams =
    & {
      id: string;
      username: string;
    };
  export type OrgsGetTeamReposParams =
    & {
      id: string;
      page?: number;
      per_page?: number;
    };
  export type OrgsGetPendingTeamInvitesParams =
    & {
      id: string;
      page?: number;
      per_page?: number;
    };
  export type OrgsCheckTeamRepoParams =
    & {
      id: string;
      owner: string;
      repo: string;
    };
  export type OrgsAddTeamRepoParams =
    & {
      id: string;
      org: string;
      repo: string;
      permission?: "pull"|"push"|"admin";
    };
  export type OrgsDeleteTeamRepoParams =
    & {
      id: string;
      owner: string;
      repo: string;
    };
  export type OrgsGetHooksParams =
    & {
      org: string;
      page?: number;
      per_page?: number;
    };
  export type OrgsGetHookParams =
    & {
      org: string;
      id: string;
    };
  export type OrgsCreateHookParams =
    & {
      org: string;
      name: string;
      config: json;
      events?: string[];
      active?: boolean;
    };
  export type OrgsEditHookParams =
    & {
      org: string;
      id: string;
      config: json;
      events?: string[];
      active?: boolean;
    };
  export type OrgsPingHookParams =
    & {
      org: string;
      id: string;
    };
  export type OrgsDeleteHookParams =
    & {
      org: string;
      id: string;
    };
  export type OrgsGetBlockedUsersParams =
    & {
      org: string;
      page?: number;
      per_page?: number;
    };
  export type OrgsCheckBlockedUserParams =
    & {
      org: string;
      username: string;
    };
  export type OrgsBlockUserParams =
    & {
      org: string;
      username: string;
    };
  export type OrgsUnblockUserParams =
    & {
      org: string;
      username: string;
    };
  export type ProjectsGetRepoProjectsParams =
    & {
      owner: string;
      repo: string;
      state?: "open"|"closed"|"all";
    };
  export type ProjectsGetOrgProjectsParams =
    & {
      org: string;
      state?: "open"|"closed"|"all";
    };
  export type ProjectsGetProjectParams =
    & {
      id: string;
    };
  export type ProjectsCreateRepoProjectParams =
    & {
      owner: string;
      repo: string;
      name: string;
      body?: string;
    };
  export type ProjectsCreateOrgProjectParams =
    & {
      org: string;
      name: string;
      body?: string;
    };
  export type ProjectsUpdateProjectParams =
    & {
      id: string;
      name: string;
      body?: string;
      state?: "open"|"closed"|"all";
    };
  export type ProjectsDeleteProjectParams =
    & {
      id: string;
    };
  export type ProjectsGetProjectCardsParams =
    & {
      column_id: string;
    };
  export type ProjectsGetProjectCardParams =
    & {
      id: string;
    };
  export type ProjectsCreateProjectCardParams =
    & {
      column_id: string;
      note?: string;
      content_id?: string;
      content_type?: string;
    };
  export type ProjectsUpdateProjectCardParams =
    & {
      id: string;
      note?: string;
    };
  export type ProjectsDeleteProjectCardParams =
    & {
      id: string;
    };
  export type ProjectsMoveProjectCardParams =
    & {
      id: string;
      position: string;
      column_id?: string;
    };
  export type ProjectsGetProjectColumnsParams =
    & {
      project_id: string;
    };
  export type ProjectsGetProjectColumnParams =
    & {
      id: string;
    };
  export type ProjectsCreateProjectColumnParams =
    & {
      project_id: string;
      name: string;
    };
  export type ProjectsUpdateProjectColumnParams =
    & {
      id: string;
      name: string;
    };
  export type ProjectsDeleteProjectColumnParams =
    & {
      id: string;
    };
  export type ProjectsMoveProjectColumnParams =
    & {
      id: string;
      position: string;
    };
  export type PullRequestsGetParams =
    & {
      owner: string;
      repo: string;
      number: number;
    };
  export type PullRequestsCreateParams =
    & {
      owner: string;
      repo: string;
      head: string;
      base: string;
    };
  export type PullRequestsUpdateParams =
    & {
      owner: string;
      repo: string;
      number: number;
      title?: string;
      body?: string;
      state?: "open"|"closed";
      base?: string;
      maintainer_can_modify?: boolean;
    };
  export type PullRequestsMergeParams =
    & {
      owner: string;
      repo: string;
      number: number;
      commit_title?: string;
      commit_message?: string;
      sha?: string;
      merge_method?: "merge"|"squash"|"rebase";
    };
  export type PullRequestsGetAllParams =
    & {
      owner: string;
      repo: string;
      state?: "open"|"closed"|"all";
      head?: string;
      base?: string;
      sort?: "created"|"updated"|"popularity"|"long-running";
      direction?: "asc"|"desc";
      page?: number;
      per_page?: number;
    };
  export type PullRequestsCreateFromIssueParams =
    & {
      owner: string;
      repo: string;
      issue: number;
      head: string;
      base: string;
    };
  export type PullRequestsGetCommitsParams =
    & {
      owner: string;
      repo: string;
      number: number;
      page?: number;
      per_page?: number;
    };
  export type PullRequestsGetFilesParams =
    & {
      owner: string;
      repo: string;
      number: number;
      page?: number;
      per_page?: number;
    };
  export type PullRequestsCheckMergedParams =
    & {
      owner: string;
      repo: string;
      number: number;
      page?: number;
      per_page?: number;
    };
  export type PullRequestsGetReviewsParams =
    & {
      owner: string;
      repo: string;
      number: number;
      page?: number;
      per_page?: number;
    };
  export type PullRequestsGetReviewParams =
    & {
      owner: string;
      repo: string;
      number: number;
      id: string;
    };
  export type PullRequestsDeletePendingReviewParams =
    & {
      owner: string;
      repo: string;
      number: number;
      id: string;
    };
  export type PullRequestsGetReviewCommentsParams =
    & {
      owner: string;
      repo: string;
      number: number;
      id: string;
      page?: number;
      per_page?: number;
    };
  export type PullRequestsCreateReviewParams =
    & {
      owner: string;
      repo: string;
      number: number;
      commit_id?: string;
      body?: string;
      event?: "APPROVE"|"REQUEST_CHANGES"|"COMMENT"|"PENDING";
      comments?: string[];
    };
  export type PullRequestsSubmitReviewParams =
    & {
      owner: string;
      repo: string;
      number: number;
      id: string;
      body?: string;
      event?: "APPROVE"|"REQUEST_CHANGES"|"COMMENT"|"PENDING";
    };
  export type PullRequestsDismissReviewParams =
    & {
      owner: string;
      repo: string;
      number: number;
      id: string;
      message?: string;
      page?: number;
      per_page?: number;
    };
  export type PullRequestsGetCommentsParams =
    & {
      owner: string;
      repo: string;
      number: number;
      page?: number;
      per_page?: number;
    };
  export type PullRequestsGetCommentsForRepoParams =
    & {
      owner: string;
      repo: string;
      sort?: "created"|"updated";
      direction?: "asc"|"desc";
      since?: date;
      page?: number;
      per_page?: number;
    };
  export type PullRequestsGetCommentParams =
    & {
      owner: string;
      repo: string;
      id: string;
    };
  export type PullRequestsCreateCommentParams =
    & {
      owner: string;
      repo: string;
      number: number;
      body: string;
    };
  export type PullRequestsCreateCommentReplyParams =
    & {
      owner: string;
      repo: string;
      number: number;
      body: string;
      in_reply_to: number;
    };
  export type PullRequestsEditCommentParams =
    & {
      owner: string;
      repo: string;
      id: string;
      body: string;
    };
  export type PullRequestsDeleteCommentParams =
    & {
      owner: string;
      repo: string;
      id: string;
    };
  export type PullRequestsGetReviewRequestsParams =
    & {
      owner: string;
      repo: string;
      number: number;
      page?: number;
      per_page?: number;
    };
  export type PullRequestsCreateReviewRequestParams =
    & {
      owner: string;
      repo: string;
      number: number;
      reviewers?: string[];
      team_reviewers?: string[];
    };
  export type PullRequestsDeleteReviewRequestParams =
    & {
      owner: string;
      repo: string;
      number: number;
      reviewers?: string[];
      team_reviewers?: string[];
    };
  export type ReactionsDeleteParams =
    & {
      id: string;
    };
  export type ReactionsGetForCommitCommentParams =
    & {
      owner: string;
      repo: string;
      id: string;
      content?: "+1"|"-1"|"laugh"|"confused"|"heart"|"hooray";
    };
  export type ReactionsCreateForCommitCommentParams =
    & {
      owner: string;
      repo: string;
      id: string;
      content: "+1"|"-1"|"laugh"|"confused"|"heart"|"hooray";
    };
  export type ReactionsGetForIssueParams =
    & {
      owner: string;
      repo: string;
      number: number;
      content?: "+1"|"-1"|"laugh"|"confused"|"heart"|"hooray";
    };
  export type ReactionsCreateForIssueParams =
    & {
      owner: string;
      repo: string;
      number: number;
      content: "+1"|"-1"|"laugh"|"confused"|"heart"|"hooray";
    };
  export type ReactionsGetForIssueCommentParams =
    & {
      owner: string;
      repo: string;
      id: string;
      content?: "+1"|"-1"|"laugh"|"confused"|"heart"|"hooray";
    };
  export type ReactionsCreateForIssueCommentParams =
    & {
      owner: string;
      repo: string;
      id: string;
      content: "+1"|"-1"|"laugh"|"confused"|"heart"|"hooray";
    };
  export type ReactionsGetForPullRequestReviewCommentParams =
    & {
      owner: string;
      repo: string;
      id: string;
      content?: "+1"|"-1"|"laugh"|"confused"|"heart"|"hooray";
    };
  export type ReactionsCreateForPullRequestReviewCommentParams =
    & {
      owner: string;
      repo: string;
      id: string;
      content: "+1"|"-1"|"laugh"|"confused"|"heart"|"hooray";
    };
  export type ReposCreateParams =
    & {
      name: string;
      description?: string;
      homepage?: string;
      private?: boolean;
      has_issues?: boolean;
      has_projects?: boolean;
      has_wiki?: boolean;
      team_id?: number;
      auto_init?: boolean;
      gitignore_template?: string;
      license_template?: string;
      allow_squash_merge?: boolean;
      allow_merge_commit?: boolean;
      allow_rebase_merge?: boolean;
    };
  export type ReposGetParams =
    & {
      owner: string;
      repo: string;
    };
  export type ReposEditParams =
    & {
      owner: string;
      repo: string;
      name: string;
      description?: string;
      homepage?: string;
      private?: boolean;
      has_issues?: boolean;
      has_projects?: boolean;
      has_wiki?: boolean;
      default_branch?: string;
      allow_squash_merge?: boolean;
      allow_merge_commit?: boolean;
      allow_rebase_merge?: boolean;
    };
  export type ReposDeleteParams =
    & {
      owner: string;
      repo: string;
    };
  export type ReposForkParams =
    & {
      owner: string;
      repo: string;
      organization?: string;
    };
  export type ReposMergeParams =
    & {
      owner: string;
      repo: string;
      base: string;
      head: string;
      commit_message?: string;
    };
  export type ReposGetAllParams =
    & {
      visibility?: "all"|"public"|"private";
      affiliation?: string;
      type?: "all"|"owner"|"public"|"private"|"member";
      sort?: "created"|"updated"|"pushed"|"full_name";
      direction?: "asc"|"desc";
      page?: number;
      per_page?: number;
    };
  export type ReposGetForUserParams =
    & {
      username: string;
      type?: "all"|"owner"|"member";
      sort?: "created"|"updated"|"pushed"|"full_name";
      direction?: "asc"|"desc";
      page?: number;
      per_page?: number;
    };
  export type ReposGetForOrgParams =
    & {
      org: string;
      type?: "all"|"public"|"private"|"forks"|"sources"|"member";
      page?: number;
      per_page?: number;
    };
  export type ReposGetPublicParams =
    & {
      since?: string;
      page?: number;
      per_page?: number;
    };
  export type ReposCreateForOrgParams =
    & {
      org: string;
      name: string;
      description?: string;
      homepage?: string;
      private?: boolean;
      has_issues?: boolean;
      has_projects?: boolean;
      has_wiki?: boolean;
      team_id?: number;
      auto_init?: boolean;
      gitignore_template?: string;
      license_template?: string;
      allow_squash_merge?: boolean;
      allow_merge_commit?: boolean;
      allow_rebase_merge?: boolean;
    };
  export type ReposGetByIdParams =
    & {
      id: string;
    };
  export type ReposGetTopicsParams =
    & {
      owner: string;
      repo: string;
      page?: number;
      per_page?: number;
    };
  export type ReposReplaceTopicsParams =
    & {
      owner: string;
      repo: string;
      names: string[];
    };
  export type ReposGetContributorsParams =
    & {
      owner: string;
      repo: string;
      anon?: boolean;
      page?: number;
      per_page?: number;
    };
  export type ReposGetLanguagesParams =
    & {
      owner: string;
      repo: string;
      page?: number;
      per_page?: number;
    };
  export type ReposGetTeamsParams =
    & {
      owner: string;
      repo: string;
      page?: number;
      per_page?: number;
    };
  export type ReposGetTagsParams =
    & {
      owner: string;
      repo: string;
      page?: number;
      per_page?: number;
    };
  export type ReposGetBranchesParams =
    & {
      owner: string;
      repo: string;
      protected?: boolean;
      page?: number;
      per_page?: number;
    };
  export type ReposGetBranchParams =
    & {
      owner: string;
      repo: string;
      branch: string;
      page?: number;
      per_page?: number;
    };
  export type ReposGetBranchProtectionParams =
    & {
      owner: string;
      repo: string;
      branch: string;
      page?: number;
      per_page?: number;
    };
  export type ReposUpdateBranchProtectionParams =
    & {
      owner: string;
      repo: string;
      branch: string;
      required_status_checks: json;
      required_pull_request_reviews: json;
      dismissal_restrictions?: json;
      restrictions: json;
      enforce_admins: boolean;
      page?: number;
      per_page?: number;
    };
  export type ReposRemoveBranchProtectionParams =
    & {
      owner: string;
      repo: string;
      branch: string;
    };
  export type ReposGetProtectedBranchRequiredStatusChecksParams =
    & {
      owner: string;
      repo: string;
      branch: string;
      page?: number;
      per_page?: number;
    };
  export type ReposUpdateProtectedBranchRequiredStatusChecksParams =
    & {
      owner: string;
      repo: string;
      branch: string;
      strict?: boolean;
      contexts?: string[];
    };
  export type ReposRemoveProtectedBranchRequiredStatusChecksParams =
    & {
      owner: string;
      repo: string;
      branch: string;
    };
  export type ReposGetProtectedBranchRequiredStatusChecksContextsParams =
    & {
      owner: string;
      repo: string;
      branch: string;
      page?: number;
      per_page?: number;
    };
  export type ReposReplaceProtectedBranchRequiredStatusChecksContextsParams =
    & {
      owner: string;
      repo: string;
      branch: string;
      contexts: string[];
    };
  export type ReposAddProtectedBranchRequiredStatusChecksContextsParams =
    & {
      owner: string;
      repo: string;
      branch: string;
      contexts: string[];
    };
  export type ReposRemoveProtectedBranchRequiredStatusChecksContextsParams =
    & {
      owner: string;
      repo: string;
      branch: string;
      contexts: string[];
    };
  export type ReposGetProtectedBranchPullRequestReviewEnforcementParams =
    & {
      owner: string;
      repo: string;
      branch: string;
      page?: number;
      per_page?: number;
    };
  export type ReposUpdateProtectedBranchPullRequestReviewEnforcementParams =
    & {
      owner: string;
      repo: string;
      branch: string;
      dismissal_restrictions?: json;
      dismiss_stale_reviews?: boolean;
      require_code_owner_reviews?: boolean;
    };
  export type ReposRemoveProtectedBranchPullRequestReviewEnforcementParams =
    & {
      owner: string;
      repo: string;
      branch: string;
    };
  export type ReposGetProtectedBranchAdminEnforcementParams =
    & {
      owner: string;
      repo: string;
      branch: string;
      page?: number;
      per_page?: number;
    };
  export type ReposAddProtectedBranchAdminEnforcementParams =
    & {
      owner: string;
      repo: string;
      branch: string;
      page?: number;
      per_page?: number;
    };
  export type ReposRemoveProtectedBranchAdminEnforcementParams =
    & {
      owner: string;
      repo: string;
      branch: string;
    };
  export type ReposGetProtectedBranchRestrictionsParams =
    & {
      owner: string;
      repo: string;
      branch: string;
      page?: number;
      per_page?: number;
    };
  export type ReposRemoveProtectedBranchRestrictionsParams =
    & {
      owner: string;
      repo: string;
      branch: string;
    };
  export type ReposGetProtectedBranchTeamRestrictionsParams =
    & {
      owner: string;
      repo: string;
      branch: string;
      page?: number;
      per_page?: number;
    };
  export type ReposReplaceProtectedBranchTeamRestrictionsParams =
    & {
      owner: string;
      repo: string;
      branch: string;
      teams: string[];
    };
  export type ReposAddProtectedBranchTeamRestrictionsParams =
    & {
      owner: string;
      repo: string;
      branch: string;
      teams: string[];
    };
  export type ReposRemoveProtectedBranchTeamRestrictionsParams =
    & {
      owner: string;
      repo: string;
      branch: string;
      teams: string[];
    };
  export type ReposGetProtectedBranchUserRestrictionsParams =
    & {
      owner: string;
      repo: string;
      branch: string;
      page?: number;
      per_page?: number;
    };
  export type ReposReplaceProtectedBranchUserRestrictionsParams =
    & {
      owner: string;
      repo: string;
      branch: string;
      users: string[];
    };
  export type ReposAddProtectedBranchUserRestrictionsParams =
    & {
      owner: string;
      repo: string;
      branch: string;
      users: string[];
    };
  export type ReposRemoveProtectedBranchUserRestrictionsParams =
    & {
      owner: string;
      repo: string;
      branch: string;
      users: string[];
    };
  export type ReposGetCollaboratorsParams =
    & {
      owner: string;
      repo: string;
      affiliation?: "outside"|"all"|"direct";
      page?: number;
      per_page?: number;
    };
  export type ReposCheckCollaboratorParams =
    & {
      owner: string;
      repo: string;
      username: string;
    };
  export type ReposReviewUserPermissionLevelParams =
    & {
      owner: string;
      repo: string;
      username: string;
    };
  export type ReposAddCollaboratorParams =
    & {
      owner: string;
      repo: string;
      username: string;
      permission?: "pull"|"push"|"admin";
    };
  export type ReposRemoveCollaboratorParams =
    & {
      owner: string;
      repo: string;
      username: string;
    };
  export type ReposGetAllCommitCommentsParams =
    & {
      owner: string;
      repo: string;
      page?: number;
      per_page?: number;
    };
  export type ReposGetCommitCommentsParams =
    & {
      owner: string;
      repo: string;
      ref: string;
      page?: number;
      per_page?: number;
    };
  export type ReposCreateCommitCommentParams =
    & {
      owner: string;
      repo: string;
      sha: string;
      body: string;
      path?: string;
      position?: number;
    };
  export type ReposGetCommitCommentParams =
    & {
      owner: string;
      repo: string;
      id: string;
    };
  export type ReposUpdateCommitCommentParams =
    & {
      owner: string;
      repo: string;
      id: string;
      body: string;
    };
  export type ReposDeleteCommitCommentParams =
    & {
      owner: string;
      repo: string;
      id: string;
    };
  export type ReposGetCommunityProfileMetricsParams =
    & {
      owner: string;
      name: string;
    };
  export type ReposGetCommitsParams =
    & {
      owner: string;
      repo: string;
      sha?: string;
      path?: string;
      author?: string;
      since?: date;
      until?: date;
      page?: number;
      per_page?: number;
    };
  export type ReposGetCommitParams =
    & {
      owner: string;
      repo: string;
      sha: string;
    };
  export type ReposGetShaOfCommitRefParams =
    & {
      owner: string;
      repo: string;
      ref: string;
    };
  export type ReposCompareCommitsParams =
    & {
      owner: string;
      repo: string;
      base: string;
      head: string;
    };
  export type ReposGetReadmeParams =
    & {
      owner: string;
      repo: string;
      ref?: string;
    };
  export type ReposGetContentParams =
    & {
      owner: string;
      repo: string;
      path: string;
      ref?: string;
    };
  export type ReposCreateFileParams =
    & {
      owner: string;
      repo: string;
      path: string;
      message: string;
      content: string;
      branch?: string;
      committer?: json;
      author?: json;
    };
  export type ReposUpdateFileParams =
    & {
      owner: string;
      repo: string;
      path: string;
      message: string;
      content: string;
      sha: string;
      branch?: string;
      committer?: json;
      author?: json;
    };
  export type ReposDeleteFileParams =
    & {
      owner: string;
      repo: string;
      path: string;
      message: string;
      sha: string;
      branch?: string;
      committer?: json;
      author?: json;
    };
  export type ReposGetArchiveLinkParams =
    & {
      owner: string;
      repo: string;
      archive_format: "tarball"|"zipball";
      ref?: string;
    };
  export type ReposGetDeployKeysParams =
    & {
      owner: string;
      repo: string;
      page?: number;
      per_page?: number;
    };
  export type ReposGetDeployKeyParams =
    & {
      owner: string;
      repo: string;
      id: string;
    };
  export type ReposAddDeployKeyParams =
    & {
      owner: string;
      repo: string;
      title: string;
      key: string;
      read_only?: boolean;
    };
  export type ReposDeleteDeployKeyParams =
    & {
      owner: string;
      repo: string;
      id: string;
    };
  export type ReposGetDeploymentsParams =
    & {
      owner: string;
      repo: string;
      sha?: string;
      ref?: string;
      task?: string;
      environment?: string;
      page?: number;
      per_page?: number;
    };
  export type ReposGetDeploymentParams =
    & {
      owner: string;
      repo: string;
      deployment_id: string;
    };
  export type ReposCreateDeploymentParams =
    & {
      owner: string;
      repo: string;
      ref: string;
      task?: string;
      auto_merge?: boolean;
      required_contexts?: string[];
      payload?: string;
      environment?: string;
      description?: string;
      transient_environment?: boolean;
      production_environment?: boolean;
    };
  export type ReposGetDeploymentStatusesParams =
    & {
      owner: string;
      repo: string;
      id: string;
    };
  export type ReposGetDeploymentStatusParams =
    & {
      owner: string;
      repo: string;
      id: string;
      status_id: string;
    };
  export type ReposCreateDeploymentStatusParams =
    & {
      owner: string;
      repo: string;
      id: string;
      state?: string;
      target_url?: string;
      log_url?: string;
      description?: string;
      environment_url?: string;
      auto_inactive?: boolean;
    };
  export type ReposGetDownloadsParams =
    & {
      owner: string;
      repo: string;
      page?: number;
      per_page?: number;
    };
  export type ReposGetDownloadParams =
    & {
      owner: string;
      repo: string;
      id: string;
    };
  export type ReposDeleteDownloadParams =
    & {
      owner: string;
      repo: string;
      id: string;
    };
  export type ReposGetForksParams =
    & {
      owner: string;
      repo: string;
      sort?: "newest"|"oldest"|"stargazers";
      page?: number;
      per_page?: number;
    };
  export type ReposGetInvitesParams =
    & {
      owner: string;
      repo: string;
    };
  export type ReposDeleteInviteParams =
    & {
      owner: string;
      repo: string;
      invitation_id: string;
    };
  export type ReposUpdateInviteParams =
    & {
      owner: string;
      repo: string;
      invitation_id: string;
      permissions?: "read"|"write"|"admin";
    };
  export type ReposGetPagesParams =
    & {
      owner: string;
      repo: string;
      page?: number;
      per_page?: number;
    };
  export type ReposRequestPageBuildParams =
    & {
      owner: string;
      repo: string;
    };
  export type ReposGetPagesBuildsParams =
    & {
      owner: string;
      repo: string;
      page?: number;
      per_page?: number;
    };
  export type ReposGetLatestPagesBuildParams =
    & {
      owner: string;
      repo: string;
    };
  export type ReposGetPagesBuildParams =
    & {
      owner: string;
      repo: string;
      id: string;
    };
  export type ReposGetReleasesParams =
    & {
      owner: string;
      repo: string;
      page?: number;
      per_page?: number;
    };
  export type ReposGetReleaseParams =
    & {
      owner: string;
      repo: string;
      id: string;
    };
  export type ReposGetLatestReleaseParams =
    & {
      owner: string;
      repo: string;
    };
  export type ReposGetReleaseByTagParams =
    & {
      owner: string;
      repo: string;
      tag: string;
    };
  export type ReposCreateReleaseParams =
    & {
      owner: string;
      repo: string;
      tag_name: string;
      target_commitish?: string;
      name?: string;
      body?: string;
      draft?: boolean;
      prerelease?: boolean;
    };
  export type ReposEditReleaseParams =
    & {
      owner: string;
      repo: string;
      id: string;
      tag_name: string;
      target_commitish?: string;
      name?: string;
      body?: string;
      draft?: boolean;
      prerelease?: boolean;
    };
  export type ReposDeleteReleaseParams =
    & {
      owner: string;
      repo: string;
      id: string;
    };
  export type ReposGetAssetsParams =
    & {
      owner: string;
      repo: string;
      id: string;
    };
  export type ReposUploadAssetParams =
    & {
      url: string;
      file: string | object;
      contentType: string;
      contentLength: number;
      name: string;
      label?: string;
    };
  export type ReposGetAssetParams =
    & {
      owner: string;
      repo: string;
      id: string;
    };
  export type ReposEditAssetParams =
    & {
      owner: string;
      repo: string;
      id: string;
      name: string;
      label?: string;
    };
  export type ReposDeleteAssetParams =
    & {
      owner: string;
      repo: string;
      id: string;
    };
  export type ReposGetStatsContributorsParams =
    & {
      owner: string;
      repo: string;
    };
  export type ReposGetStatsCommitActivityParams =
    & {
      owner: string;
      repo: string;
    };
  export type ReposGetStatsCodeFrequencyParams =
    & {
      owner: string;
      repo: string;
    };
  export type ReposGetStatsParticipationParams =
    & {
      owner: string;
      repo: string;
    };
  export type ReposGetStatsPunchCardParams =
    & {
      owner: string;
      repo: string;
    };
  export type ReposCreateStatusParams =
    & {
      owner: string;
      repo: string;
      sha: string;
      state: "pending"|"success"|"error"|"failure";
      target_url?: string;
      description?: string;
      context?: string;
    };
  export type ReposGetStatusesParams =
    & {
      owner: string;
      repo: string;
      ref: string;
      page?: number;
      per_page?: number;
    };
  export type ReposGetCombinedStatusForRefParams =
    & {
      owner: string;
      repo: string;
      ref: string;
      page?: number;
      per_page?: number;
    };
  export type ReposGetReferrersParams =
    & {
      owner: string;
      repo: string;
      page?: number;
      per_page?: number;
    };
  export type ReposGetPathsParams =
    & {
      owner: string;
      repo: string;
      page?: number;
      per_page?: number;
    };
  export type ReposGetViewsParams =
    & {
      owner: string;
      repo: string;
      page?: number;
      per_page?: number;
    };
  export type ReposGetClonesParams =
    & {
      owner: string;
      repo: string;
      page?: number;
      per_page?: number;
    };
  export type ReposGetHooksParams =
    & {
      owner: string;
      repo: string;
      page?: number;
      per_page?: number;
    };
  export type ReposGetHookParams =
    & {
      owner: string;
      repo: string;
      id: string;
    };
  export type ReposCreateHookParams =
    & {
      owner: string;
      repo: string;
      name: string;
      config: json;
      events?: string[];
      active?: boolean;
    };
  export type ReposEditHookParams =
    & {
      owner: string;
      repo: string;
      id: string;
      name: string;
      config: json;
      events?: string[];
      add_events?: string[];
      remove_events?: string[];
      active?: boolean;
    };
  export type ReposTestHookParams =
    & {
      owner: string;
      repo: string;
      id: string;
    };
  export type ReposPingHookParams =
    & {
      owner: string;
      repo: string;
      id: string;
    };
  export type ReposDeleteHookParams =
    & {
      owner: string;
      repo: string;
      id: string;
    };
  export type SearchReposParams =
    & {
      q: string;
      sort?: "stars"|"forks"|"updated";
      order?: "asc"|"desc";
      page?: number;
      per_page?: number;
    };
  export type SearchCodeParams =
    & {
      q: string;
      sort?: "indexed";
      order?: "asc"|"desc";
      page?: number;
      per_page?: number;
    };
  export type SearchCommitsParams =
    & {
      q: string;
      sort?: "author-date"|"committer-date";
      order?: "asc"|"desc";
      page?: number;
      per_page?: number;
    };
  export type SearchIssuesParams =
    & {
      q: string;
      sort?: "comments"|"created"|"updated";
      order?: "asc"|"desc";
      page?: number;
      per_page?: number;
    };
  export type SearchUsersParams =
    & {
      q: string;
      sort?: "followers"|"repositories"|"joined";
      order?: "asc"|"desc";
      page?: number;
      per_page?: number;
    };
  export type SearchEmailParams =
    & {
      email: string;
    };
  export type UsersUpdateParams =
    & {
      name?: string;
      email?: string;
      blog?: string;
      company?: string;
      location?: string;
      hireable?: boolean;
      bio?: string;
    };
  export type UsersPromoteParams =
    & {
      username: string;
    };
  export type UsersDemoteParams =
    & {
      username: string;
    };
  export type UsersSuspendParams =
    & {
      username: string;
    };
  export type UsersUnsuspendParams =
    & {
      username: string;
    };
  export type UsersGetForUserParams =
    & {
      username: string;
    };
  export type UsersGetByIdParams =
    & {
      id: string;
    };
  export type UsersGetAllParams =
    & {
      since?: number;
    };
  export type UsersGetOrgsParams =
    & {
      page?: number;
      per_page?: number;
    };
  export type UsersGetOrgMembershipsParams =
    & {
      state?: "active"|"pending";
    };
  export type UsersGetOrgMembershipParams =
    & {
      org: string;
    };
  export type UsersEditOrgMembershipParams =
    & {
      org: string;
      state: "active";
    };
  export type UsersGetTeamsParams =
    & {
      page?: number;
      per_page?: number;
    };
  export type UsersGetEmailsParams =
    & {
      page?: number;
      per_page?: number;
    };
  export type UsersGetPublicEmailsParams =
    & {
      page?: number;
      per_page?: number;
    };
  export type UsersAddEmailsParams =
    & {
      emails: string[];
    };
  export type UsersDeleteEmailsParams =
    & {
      emails: string[];
    };
  export type UsersGetFollowersForUserParams =
    & {
      username: string;
      page?: number;
      per_page?: number;
    };
  export type UsersGetFollowersParams =
    & {
      page?: number;
      per_page?: number;
    };
  export type UsersGetFollowingForUserParams =
    & {
      username: string;
      page?: number;
      per_page?: number;
    };
  export type UsersGetFollowingParams =
    & {
      page?: number;
      per_page?: number;
    };
  export type UsersCheckFollowingParams =
    & {
      username: string;
    };
  export type UsersCheckIfOneFollowersOtherParams =
    & {
      username: string;
      target_user: string;
    };
  export type UsersFollowUserParams =
    & {
      username: string;
    };
  export type UsersUnfollowUserParams =
    & {
      username: string;
    };
  export type UsersGetKeysForUserParams =
    & {
      username: string;
      page?: number;
      per_page?: number;
    };
  export type UsersGetKeysParams =
    & {
      page?: number;
      per_page?: number;
    };
  export type UsersGetKeyParams =
    & {
      id: string;
    };
  export type UsersCreateKeyParams =
    & {
      title: string;
      key: string;
    };
  export type UsersDeleteKeyParams =
    & {
      id: string;
    };
  export type UsersGetGpgKeysForUserParams =
    & {
      username: string;
      page?: number;
      per_page?: number;
    };
  export type UsersGetGpgKeysParams =
    & {
      page?: number;
      per_page?: number;
    };
  export type UsersGetGpgKeyParams =
    & {
      id: string;
    };
  export type UsersCreateGpgKeyParams =
    & {
      armored_public_key: string;
    };
  export type UsersDeleteGpgKeyParams =
    & {
      id: string;
    };
  export type UsersCheckBlockedUserParams =
    & {
      username: string;
    };
  export type UsersBlockUserParams =
    & {
      username: string;
    };
  export type UsersUnblockUserParams =
    & {
      username: string;
    };
  export type UsersAcceptRepoInviteParams =
    & {
      invitation_id: string;
    };
  export type UsersDeclineRepoInviteParams =
    & {
      invitation_id: string;
    };
  export type UsersGetInstallationsParams =
    & {
      page?: number;
      per_page?: number;
    };
  export type UsersGetInstallationReposParams =
    & {
      installation_id: string;
      page?: number;
      per_page?: number;
    };
  export type UsersAddRepoToInstallationParams =
    & {
      installation_id: string;
      repository_id: string;
    };
  export type UsersRemoveRepoFromInstallationParams =
    & {
      installation_id: string;
      repository_id: string;
    };
  export type UsersGetMarketplacePurchasesParams =
    & {
      page?: number;
      per_page?: number;
    };
  export type UsersGetMarketplaceStubbedPurchasesParams =
    & {
      page?: number;
      per_page?: number;
    };
  export type EnterpriseStatsParams =
    & {
      type: "issues"|"hooks"|"milestones"|"orgs"|"comments"|"pages"|"users"|"gists"|"pulls"|"repos"|"all";
    };
  export type EnterpriseUpdateLdapForUserParams =
    & {
      username: string;
      ldap_dn: string;
    };
  export type EnterpriseSyncLdapForUserParams =
    & {
      username: string;
    };
  export type EnterpriseUpdateLdapForTeamParams =
    & {
      team_id: number;
      ldap_dn: string;
    };
  export type EnterpriseSyncLdapForTeamParams =
    & {
      team_id: number;
    };
  export type EnterpriseGetPreReceiveEnvironmentParams =
    & {
      id: string;
    };
  export type EnterpriseCreatePreReceiveEnvironmentParams =
    & {
      name: string;
      image_url: string;
    };
  export type EnterpriseEditPreReceiveEnvironmentParams =
    & {
      id: string;
      name: string;
      image_url: string;
    };
  export type EnterpriseDeletePreReceiveEnvironmentParams =
    & {
      id: string;
    };
  export type EnterpriseGetPreReceiveEnvironmentDownloadStatusParams =
    & {
      id: string;
    };
  export type EnterpriseTriggerPreReceiveEnvironmentDownloadParams =
    & {
      id: string;
    };
  export type EnterpriseGetPreReceiveHookParams =
    & {
      id: string;
    };
  export type EnterpriseCreatePreReceiveHookParams =
    & {
      name: string;
      script: string;
      script_repository: json;
      environment: json;
      enforcement?: string;
      allow_downstream_configuration?: boolean;
    };
  export type EnterpriseEditPreReceiveHookParams =
    & {
      id: string;
      hook: json;
    };
  export type EnterpriseDeletePreReceiveHookParams =
    & {
      id: string;
    };
  export type EnterpriseQueueIndexingJobParams =
    & {
      target: string;
    };
  export type EnterpriseCreateOrgParams =
    & {
      login: string;
      admin: string;
      profile_name?: string;
    };
}

declare class Github {
  constructor(options?: Github.Options);
  authenticate(auth: Github.Auth): void;
  hasNextPage(link: Github.Link): string | undefined;
  hasPreviousPage(link: Github.Link): string | undefined;
  hasLastPage(link: Github.Link): string | undefined;
  hasFirstPage(link: Github.Link): string | undefined;

  getNextPage(link: Github.Link, callback?: Github.Callback): Promise<Github.AnyResponse>;
  getNextPage(link: Github.Link, headers?: {[header: string]: any}, callback?: Github.Callback): Promise<Github.AnyResponse>;

  getPreviousPage(link: Github.Link, callback?: Github.Callback): Promise<Github.AnyResponse>;
  getPreviousPage(link: Github.Link, headers?: {[header: string]: any}, callback?: Github.Callback): Promise<Github.AnyResponse>;

  getLastPage(link: Github.Link, callback?: Github.Callback): Promise<Github.AnyResponse>;
  getLastPage(link: Github.Link, headers?: {[header: string]: any}, callback?: Github.Callback): Promise<Github.AnyResponse>;

  getFirstPage(link: Github.Link, callback?: Github.Callback): Promise<Github.AnyResponse>;
  getFirstPage(link: Github.Link, headers?: {[header: string]: any}, callback?: Github.Callback): Promise<Github.AnyResponse>;

  authorization: {
    get(params: Github.AuthorizationGetParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    create(params: Github.AuthorizationCreateParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    update(params: Github.AuthorizationUpdateParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    delete(params: Github.AuthorizationDeleteParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    check(params: Github.AuthorizationCheckParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    reset(params: Github.AuthorizationResetParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    revoke(params: Github.AuthorizationRevokeParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getGrants(params: Github.AuthorizationGetGrantsParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getGrant(params: Github.AuthorizationGetGrantParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    deleteGrant(params: Github.AuthorizationDeleteGrantParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getAll(params: Github.AuthorizationGetAllParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getOrCreateAuthorizationForApp(params: Github.AuthorizationGetOrCreateAuthorizationForAppParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getOrCreateAuthorizationForAppAndFingerprint(params: Github.AuthorizationGetOrCreateAuthorizationForAppAndFingerprintParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    revokeGrant(params: Github.AuthorizationRevokeGrantParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
  };
  activity: {
    getEvents(params: Github.ActivityGetEventsParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getEventsForRepo(params: Github.ActivityGetEventsForRepoParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getEventsForRepoIssues(params: Github.ActivityGetEventsForRepoIssuesParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getEventsForRepoNetwork(params: Github.ActivityGetEventsForRepoNetworkParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getEventsForOrg(params: Github.ActivityGetEventsForOrgParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getEventsReceived(params: Github.ActivityGetEventsReceivedParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getEventsReceivedPublic(params: Github.ActivityGetEventsReceivedPublicParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getEventsForUser(params: Github.ActivityGetEventsForUserParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getEventsForUserPublic(params: Github.ActivityGetEventsForUserPublicParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getEventsForUserOrg(params: Github.ActivityGetEventsForUserOrgParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getFeeds(params: Github.EmptyParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getNotifications(params: Github.ActivityGetNotificationsParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getNotificationsForUser(params: Github.ActivityGetNotificationsForUserParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    markNotificationsAsRead(params: Github.ActivityMarkNotificationsAsReadParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    markNotificationsAsReadForRepo(params: Github.ActivityMarkNotificationsAsReadForRepoParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getNotificationThread(params: Github.ActivityGetNotificationThreadParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    markNotificationThreadAsRead(params: Github.ActivityMarkNotificationThreadAsReadParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    checkNotificationThreadSubscription(params: Github.ActivityCheckNotificationThreadSubscriptionParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    setNotificationThreadSubscription(params: Github.ActivitySetNotificationThreadSubscriptionParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    deleteNotificationThreadSubscription(params: Github.ActivityDeleteNotificationThreadSubscriptionParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getStargazersForRepo(params: Github.ActivityGetStargazersForRepoParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getStarredReposForUser(params: Github.ActivityGetStarredReposForUserParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getStarredRepos(params: Github.ActivityGetStarredReposParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    checkStarringRepo(params: Github.ActivityCheckStarringRepoParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    starRepo(params: Github.ActivityStarRepoParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    unstarRepo(params: Github.ActivityUnstarRepoParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getWatchersForRepo(params: Github.ActivityGetWatchersForRepoParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getWatchedReposForUser(params: Github.ActivityGetWatchedReposForUserParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getWatchedRepos(params: Github.ActivityGetWatchedReposParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getRepoSubscription(params: Github.ActivityGetRepoSubscriptionParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    setRepoSubscription(params: Github.ActivitySetRepoSubscriptionParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    unwatchRepo(params: Github.ActivityUnwatchRepoParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
  };
  gists: {
    get(params: Github.GistsGetParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    create(params: Github.GistsCreateParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    edit(params: Github.GistsEditParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    star(params: Github.GistsStarParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    unstar(params: Github.GistsUnstarParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    fork(params: Github.GistsForkParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    delete(params: Github.GistsDeleteParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getForUser(params: Github.GistsGetForUserParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getAll(params: Github.GistsGetAllParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getPublic(params: Github.GistsGetPublicParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getStarred(params: Github.GistsGetStarredParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getRevision(params: Github.GistsGetRevisionParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getCommits(params: Github.GistsGetCommitsParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    checkStar(params: Github.GistsCheckStarParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getForks(params: Github.GistsGetForksParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getComments(params: Github.GistsGetCommentsParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getComment(params: Github.GistsGetCommentParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    createComment(params: Github.GistsCreateCommentParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    editComment(params: Github.GistsEditCommentParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    deleteComment(params: Github.GistsDeleteCommentParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
  };
  gitdata: {
    getBlob(params: Github.GitdataGetBlobParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    createBlob(params: Github.GitdataCreateBlobParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getCommit(params: Github.GitdataGetCommitParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    createCommit(params: Github.GitdataCreateCommitParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getCommitSignatureVerification(params: Github.GitdataGetCommitSignatureVerificationParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getReference(params: Github.GitdataGetReferenceParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getReferences(params: Github.GitdataGetReferencesParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getTags(params: Github.GitdataGetTagsParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    createReference(params: Github.GitdataCreateReferenceParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    updateReference(params: Github.GitdataUpdateReferenceParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    deleteReference(params: Github.GitdataDeleteReferenceParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getTag(params: Github.GitdataGetTagParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    createTag(params: Github.GitdataCreateTagParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getTagSignatureVerification(params: Github.GitdataGetTagSignatureVerificationParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getTree(params: Github.GitdataGetTreeParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    createTree(params: Github.GitdataCreateTreeParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
  };
  integrations: {
    getInstallations(params: Github.IntegrationsGetInstallationsParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    createInstallationToken(params: Github.IntegrationsCreateInstallationTokenParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getInstallationRepositories(params: Github.IntegrationsGetInstallationRepositoriesParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    addRepoToInstallation(params: Github.IntegrationsAddRepoToInstallationParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    removeRepoFromInstallation(params: Github.IntegrationsRemoveRepoFromInstallationParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
  };
  apps: {
    get(params: Github.EmptyParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getForSlug(params: Github.AppsGetForSlugParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getInstallations(params: Github.AppsGetInstallationsParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getInstallation(params: Github.AppsGetInstallationParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    createInstallationToken(params: Github.AppsCreateInstallationTokenParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getInstallationRepositories(params: Github.AppsGetInstallationRepositoriesParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    addRepoToInstallation(params: Github.AppsAddRepoToInstallationParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    removeRepoFromInstallation(params: Github.AppsRemoveRepoFromInstallationParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getMarketplaceListingPlans(params: Github.AppsGetMarketplaceListingPlansParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getMarketplaceListingStubbedPlans(params: Github.AppsGetMarketplaceListingStubbedPlansParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getMarketplaceListingPlanAccounts(params: Github.AppsGetMarketplaceListingPlanAccountsParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getMarketplaceListingStubbedPlanAccounts(params: Github.AppsGetMarketplaceListingStubbedPlanAccountsParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    checkMarketplaceListingAccount(params: Github.AppsCheckMarketplaceListingAccountParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    checkMarketplaceListingStubbedAccount(params: Github.AppsCheckMarketplaceListingStubbedAccountParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
  };
  issues: {
    get(params: Github.IssuesGetParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    create(params: Github.IssuesCreateParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    edit(params: Github.IssuesEditParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    lock(params: Github.IssuesLockParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    unlock(params: Github.IssuesUnlockParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getAll(params: Github.IssuesGetAllParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getForUser(params: Github.IssuesGetForUserParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getForOrg(params: Github.IssuesGetForOrgParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getForRepo(params: Github.IssuesGetForRepoParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getAssignees(params: Github.IssuesGetAssigneesParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    checkAssignee(params: Github.IssuesCheckAssigneeParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    addAssigneesToIssue(params: Github.IssuesAddAssigneesToIssueParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    removeAssigneesFromIssue(params: Github.IssuesRemoveAssigneesFromIssueParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getComments(params: Github.IssuesGetCommentsParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getCommentsForRepo(params: Github.IssuesGetCommentsForRepoParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getComment(params: Github.IssuesGetCommentParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    createComment(params: Github.IssuesCreateCommentParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    editComment(params: Github.IssuesEditCommentParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    deleteComment(params: Github.IssuesDeleteCommentParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getEvents(params: Github.IssuesGetEventsParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getEventsForRepo(params: Github.IssuesGetEventsForRepoParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getEvent(params: Github.IssuesGetEventParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getLabels(params: Github.IssuesGetLabelsParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getLabel(params: Github.IssuesGetLabelParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    createLabel(params: Github.IssuesCreateLabelParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    updateLabel(params: Github.IssuesUpdateLabelParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    deleteLabel(params: Github.IssuesDeleteLabelParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getIssueLabels(params: Github.IssuesGetIssueLabelsParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    addLabels(params: Github.IssuesAddLabelsParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    removeLabel(params: Github.IssuesRemoveLabelParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    replaceAllLabels(params: Github.IssuesReplaceAllLabelsParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    removeAllLabels(params: Github.IssuesRemoveAllLabelsParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getMilestoneLabels(params: Github.IssuesGetMilestoneLabelsParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getMilestones(params: Github.IssuesGetMilestonesParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getMilestone(params: Github.IssuesGetMilestoneParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    createMilestone(params: Github.IssuesCreateMilestoneParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    updateMilestone(params: Github.IssuesUpdateMilestoneParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    deleteMilestone(params: Github.IssuesDeleteMilestoneParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getEventsTimeline(params: Github.IssuesGetEventsTimelineParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
  };
  migrations: {
    startMigration(params: Github.MigrationsStartMigrationParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getMigrations(params: Github.MigrationsGetMigrationsParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getMigrationStatus(params: Github.MigrationsGetMigrationStatusParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getMigrationArchiveLink(params: Github.MigrationsGetMigrationArchiveLinkParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    deleteMigrationArchive(params: Github.MigrationsDeleteMigrationArchiveParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    unlockRepoLockedForMigration(params: Github.MigrationsUnlockRepoLockedForMigrationParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    startImport(params: Github.MigrationsStartImportParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getImportProgress(params: Github.MigrationsGetImportProgressParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    updateImport(params: Github.MigrationsUpdateImportParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getImportCommitAuthors(params: Github.MigrationsGetImportCommitAuthorsParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    mapImportCommitAuthor(params: Github.MigrationsMapImportCommitAuthorParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    setImportLfsPreference(params: Github.MigrationsSetImportLfsPreferenceParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getLargeImportFiles(params: Github.MigrationsGetLargeImportFilesParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    cancelImport(params: Github.MigrationsCancelImportParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
  };
  misc: {
    getCodesOfConduct(params: Github.EmptyParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getCodeOfConduct(params: Github.MiscGetCodeOfConductParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getRepoCodeOfConduct(params: Github.MiscGetRepoCodeOfConductParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getEmojis(params: Github.EmptyParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getGitignoreTemplates(params: Github.EmptyParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getGitignoreTemplate(params: Github.MiscGetGitignoreTemplateParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getLicenses(params: Github.EmptyParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getLicense(params: Github.MiscGetLicenseParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getRepoLicense(params: Github.MiscGetRepoLicenseParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    renderMarkdown(params: Github.MiscRenderMarkdownParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    renderMarkdownRaw(params: Github.MiscRenderMarkdownRawParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getMeta(params: Github.EmptyParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getRateLimit(params: Github.EmptyParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
  };
  orgs: {
    get(params: Github.OrgsGetParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    update(params: Github.OrgsUpdateParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getAll(params: Github.OrgsGetAllParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getForUser(params: Github.OrgsGetForUserParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getMembers(params: Github.OrgsGetMembersParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    checkMembership(params: Github.OrgsCheckMembershipParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    removeMember(params: Github.OrgsRemoveMemberParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getPublicMembers(params: Github.OrgsGetPublicMembersParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    checkPublicMembership(params: Github.OrgsCheckPublicMembershipParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    publicizeMembership(params: Github.OrgsPublicizeMembershipParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    concealMembership(params: Github.OrgsConcealMembershipParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getOrgMembership(params: Github.OrgsGetOrgMembershipParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    addOrgMembership(params: Github.OrgsAddOrgMembershipParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    removeOrgMembership(params: Github.OrgsRemoveOrgMembershipParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getPendingOrgInvites(params: Github.OrgsGetPendingOrgInvitesParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getOutsideCollaborators(params: Github.OrgsGetOutsideCollaboratorsParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    removeOutsideCollaborator(params: Github.OrgsRemoveOutsideCollaboratorParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    convertMemberToOutsideCollaborator(params: Github.OrgsConvertMemberToOutsideCollaboratorParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getTeams(params: Github.OrgsGetTeamsParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getTeam(params: Github.OrgsGetTeamParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    createTeam(params: Github.OrgsCreateTeamParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    editTeam(params: Github.OrgsEditTeamParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    deleteTeam(params: Github.OrgsDeleteTeamParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getTeamMembers(params: Github.OrgsGetTeamMembersParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getChildTeams(params: Github.OrgsGetChildTeamsParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getTeamMembership(params: Github.OrgsGetTeamMembershipParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    addTeamMembership(params: Github.OrgsAddTeamMembershipParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    removeTeamMembership(params: Github.OrgsRemoveTeamMembershipParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getTeamRepos(params: Github.OrgsGetTeamReposParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getPendingTeamInvites(params: Github.OrgsGetPendingTeamInvitesParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    checkTeamRepo(params: Github.OrgsCheckTeamRepoParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    addTeamRepo(params: Github.OrgsAddTeamRepoParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    deleteTeamRepo(params: Github.OrgsDeleteTeamRepoParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getHooks(params: Github.OrgsGetHooksParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getHook(params: Github.OrgsGetHookParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    createHook(params: Github.OrgsCreateHookParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    editHook(params: Github.OrgsEditHookParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    pingHook(params: Github.OrgsPingHookParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    deleteHook(params: Github.OrgsDeleteHookParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getBlockedUsers(params: Github.OrgsGetBlockedUsersParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    checkBlockedUser(params: Github.OrgsCheckBlockedUserParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    blockUser(params: Github.OrgsBlockUserParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    unblockUser(params: Github.OrgsUnblockUserParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
  };
  projects: {
    getRepoProjects(params: Github.ProjectsGetRepoProjectsParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getOrgProjects(params: Github.ProjectsGetOrgProjectsParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getProject(params: Github.ProjectsGetProjectParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    createRepoProject(params: Github.ProjectsCreateRepoProjectParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    createOrgProject(params: Github.ProjectsCreateOrgProjectParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    updateProject(params: Github.ProjectsUpdateProjectParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    deleteProject(params: Github.ProjectsDeleteProjectParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getProjectCards(params: Github.ProjectsGetProjectCardsParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getProjectCard(params: Github.ProjectsGetProjectCardParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    createProjectCard(params: Github.ProjectsCreateProjectCardParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    updateProjectCard(params: Github.ProjectsUpdateProjectCardParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    deleteProjectCard(params: Github.ProjectsDeleteProjectCardParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    moveProjectCard(params: Github.ProjectsMoveProjectCardParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getProjectColumns(params: Github.ProjectsGetProjectColumnsParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getProjectColumn(params: Github.ProjectsGetProjectColumnParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    createProjectColumn(params: Github.ProjectsCreateProjectColumnParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    updateProjectColumn(params: Github.ProjectsUpdateProjectColumnParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    deleteProjectColumn(params: Github.ProjectsDeleteProjectColumnParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    moveProjectColumn(params: Github.ProjectsMoveProjectColumnParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
  };
  pullRequests: {
    get(params: Github.PullRequestsGetParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    create(params: Github.PullRequestsCreateParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    update(params: Github.PullRequestsUpdateParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    merge(params: Github.PullRequestsMergeParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getAll(params: Github.PullRequestsGetAllParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    createFromIssue(params: Github.PullRequestsCreateFromIssueParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getCommits(params: Github.PullRequestsGetCommitsParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getFiles(params: Github.PullRequestsGetFilesParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    checkMerged(params: Github.PullRequestsCheckMergedParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getReviews(params: Github.PullRequestsGetReviewsParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getReview(params: Github.PullRequestsGetReviewParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    deletePendingReview(params: Github.PullRequestsDeletePendingReviewParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getReviewComments(params: Github.PullRequestsGetReviewCommentsParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    createReview(params: Github.PullRequestsCreateReviewParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    submitReview(params: Github.PullRequestsSubmitReviewParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    dismissReview(params: Github.PullRequestsDismissReviewParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getComments(params: Github.PullRequestsGetCommentsParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getCommentsForRepo(params: Github.PullRequestsGetCommentsForRepoParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getComment(params: Github.PullRequestsGetCommentParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    createComment(params: Github.PullRequestsCreateCommentParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    createCommentReply(params: Github.PullRequestsCreateCommentReplyParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    editComment(params: Github.PullRequestsEditCommentParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    deleteComment(params: Github.PullRequestsDeleteCommentParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getReviewRequests(params: Github.PullRequestsGetReviewRequestsParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    createReviewRequest(params: Github.PullRequestsCreateReviewRequestParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    deleteReviewRequest(params: Github.PullRequestsDeleteReviewRequestParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
  };
  reactions: {
    delete(params: Github.ReactionsDeleteParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getForCommitComment(params: Github.ReactionsGetForCommitCommentParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    createForCommitComment(params: Github.ReactionsCreateForCommitCommentParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getForIssue(params: Github.ReactionsGetForIssueParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    createForIssue(params: Github.ReactionsCreateForIssueParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getForIssueComment(params: Github.ReactionsGetForIssueCommentParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    createForIssueComment(params: Github.ReactionsCreateForIssueCommentParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getForPullRequestReviewComment(params: Github.ReactionsGetForPullRequestReviewCommentParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    createForPullRequestReviewComment(params: Github.ReactionsCreateForPullRequestReviewCommentParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
  };
  repos: {
    create(params: Github.ReposCreateParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    get(params: Github.ReposGetParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    edit(params: Github.ReposEditParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    delete(params: Github.ReposDeleteParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    fork(params: Github.ReposForkParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    merge(params: Github.ReposMergeParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getAll(params: Github.ReposGetAllParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getForUser(params: Github.ReposGetForUserParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getForOrg(params: Github.ReposGetForOrgParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getPublic(params: Github.ReposGetPublicParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    createForOrg(params: Github.ReposCreateForOrgParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getById(params: Github.ReposGetByIdParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getTopics(params: Github.ReposGetTopicsParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    replaceTopics(params: Github.ReposReplaceTopicsParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getContributors(params: Github.ReposGetContributorsParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getLanguages(params: Github.ReposGetLanguagesParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getTeams(params: Github.ReposGetTeamsParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getTags(params: Github.ReposGetTagsParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getBranches(params: Github.ReposGetBranchesParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getBranch(params: Github.ReposGetBranchParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getBranchProtection(params: Github.ReposGetBranchProtectionParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    updateBranchProtection(params: Github.ReposUpdateBranchProtectionParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    removeBranchProtection(params: Github.ReposRemoveBranchProtectionParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getProtectedBranchRequiredStatusChecks(params: Github.ReposGetProtectedBranchRequiredStatusChecksParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    updateProtectedBranchRequiredStatusChecks(params: Github.ReposUpdateProtectedBranchRequiredStatusChecksParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    removeProtectedBranchRequiredStatusChecks(params: Github.ReposRemoveProtectedBranchRequiredStatusChecksParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getProtectedBranchRequiredStatusChecksContexts(params: Github.ReposGetProtectedBranchRequiredStatusChecksContextsParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    replaceProtectedBranchRequiredStatusChecksContexts(params: Github.ReposReplaceProtectedBranchRequiredStatusChecksContextsParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    addProtectedBranchRequiredStatusChecksContexts(params: Github.ReposAddProtectedBranchRequiredStatusChecksContextsParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    removeProtectedBranchRequiredStatusChecksContexts(params: Github.ReposRemoveProtectedBranchRequiredStatusChecksContextsParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getProtectedBranchPullRequestReviewEnforcement(params: Github.ReposGetProtectedBranchPullRequestReviewEnforcementParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    updateProtectedBranchPullRequestReviewEnforcement(params: Github.ReposUpdateProtectedBranchPullRequestReviewEnforcementParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    removeProtectedBranchPullRequestReviewEnforcement(params: Github.ReposRemoveProtectedBranchPullRequestReviewEnforcementParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getProtectedBranchAdminEnforcement(params: Github.ReposGetProtectedBranchAdminEnforcementParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    addProtectedBranchAdminEnforcement(params: Github.ReposAddProtectedBranchAdminEnforcementParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    removeProtectedBranchAdminEnforcement(params: Github.ReposRemoveProtectedBranchAdminEnforcementParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getProtectedBranchRestrictions(params: Github.ReposGetProtectedBranchRestrictionsParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    removeProtectedBranchRestrictions(params: Github.ReposRemoveProtectedBranchRestrictionsParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getProtectedBranchTeamRestrictions(params: Github.ReposGetProtectedBranchTeamRestrictionsParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    replaceProtectedBranchTeamRestrictions(params: Github.ReposReplaceProtectedBranchTeamRestrictionsParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    addProtectedBranchTeamRestrictions(params: Github.ReposAddProtectedBranchTeamRestrictionsParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    removeProtectedBranchTeamRestrictions(params: Github.ReposRemoveProtectedBranchTeamRestrictionsParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getProtectedBranchUserRestrictions(params: Github.ReposGetProtectedBranchUserRestrictionsParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    replaceProtectedBranchUserRestrictions(params: Github.ReposReplaceProtectedBranchUserRestrictionsParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    addProtectedBranchUserRestrictions(params: Github.ReposAddProtectedBranchUserRestrictionsParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    removeProtectedBranchUserRestrictions(params: Github.ReposRemoveProtectedBranchUserRestrictionsParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getCollaborators(params: Github.ReposGetCollaboratorsParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    checkCollaborator(params: Github.ReposCheckCollaboratorParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    reviewUserPermissionLevel(params: Github.ReposReviewUserPermissionLevelParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    addCollaborator(params: Github.ReposAddCollaboratorParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    removeCollaborator(params: Github.ReposRemoveCollaboratorParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getAllCommitComments(params: Github.ReposGetAllCommitCommentsParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getCommitComments(params: Github.ReposGetCommitCommentsParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    createCommitComment(params: Github.ReposCreateCommitCommentParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getCommitComment(params: Github.ReposGetCommitCommentParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    updateCommitComment(params: Github.ReposUpdateCommitCommentParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    deleteCommitComment(params: Github.ReposDeleteCommitCommentParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getCommunityProfileMetrics(params: Github.ReposGetCommunityProfileMetricsParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getCommits(params: Github.ReposGetCommitsParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getCommit(params: Github.ReposGetCommitParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getShaOfCommitRef(params: Github.ReposGetShaOfCommitRefParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    compareCommits(params: Github.ReposCompareCommitsParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getReadme(params: Github.ReposGetReadmeParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getContent(params: Github.ReposGetContentParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    createFile(params: Github.ReposCreateFileParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    updateFile(params: Github.ReposUpdateFileParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    deleteFile(params: Github.ReposDeleteFileParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getArchiveLink(params: Github.ReposGetArchiveLinkParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getDeployKeys(params: Github.ReposGetDeployKeysParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getDeployKey(params: Github.ReposGetDeployKeyParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    addDeployKey(params: Github.ReposAddDeployKeyParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    deleteDeployKey(params: Github.ReposDeleteDeployKeyParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getDeployments(params: Github.ReposGetDeploymentsParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getDeployment(params: Github.ReposGetDeploymentParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    createDeployment(params: Github.ReposCreateDeploymentParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getDeploymentStatuses(params: Github.ReposGetDeploymentStatusesParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getDeploymentStatus(params: Github.ReposGetDeploymentStatusParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    createDeploymentStatus(params: Github.ReposCreateDeploymentStatusParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getDownloads(params: Github.ReposGetDownloadsParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getDownload(params: Github.ReposGetDownloadParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    deleteDownload(params: Github.ReposDeleteDownloadParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getForks(params: Github.ReposGetForksParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getInvites(params: Github.ReposGetInvitesParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    deleteInvite(params: Github.ReposDeleteInviteParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    updateInvite(params: Github.ReposUpdateInviteParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getPages(params: Github.ReposGetPagesParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    requestPageBuild(params: Github.ReposRequestPageBuildParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getPagesBuilds(params: Github.ReposGetPagesBuildsParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getLatestPagesBuild(params: Github.ReposGetLatestPagesBuildParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getPagesBuild(params: Github.ReposGetPagesBuildParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getReleases(params: Github.ReposGetReleasesParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getRelease(params: Github.ReposGetReleaseParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getLatestRelease(params: Github.ReposGetLatestReleaseParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getReleaseByTag(params: Github.ReposGetReleaseByTagParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    createRelease(params: Github.ReposCreateReleaseParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    editRelease(params: Github.ReposEditReleaseParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    deleteRelease(params: Github.ReposDeleteReleaseParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getAssets(params: Github.ReposGetAssetsParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    uploadAsset(params: Github.ReposUploadAssetParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getAsset(params: Github.ReposGetAssetParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    editAsset(params: Github.ReposEditAssetParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    deleteAsset(params: Github.ReposDeleteAssetParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getStatsContributors(params: Github.ReposGetStatsContributorsParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getStatsCommitActivity(params: Github.ReposGetStatsCommitActivityParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getStatsCodeFrequency(params: Github.ReposGetStatsCodeFrequencyParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getStatsParticipation(params: Github.ReposGetStatsParticipationParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getStatsPunchCard(params: Github.ReposGetStatsPunchCardParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    createStatus(params: Github.ReposCreateStatusParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getStatuses(params: Github.ReposGetStatusesParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getCombinedStatusForRef(params: Github.ReposGetCombinedStatusForRefParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getReferrers(params: Github.ReposGetReferrersParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getPaths(params: Github.ReposGetPathsParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getViews(params: Github.ReposGetViewsParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getClones(params: Github.ReposGetClonesParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getHooks(params: Github.ReposGetHooksParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getHook(params: Github.ReposGetHookParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    createHook(params: Github.ReposCreateHookParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    editHook(params: Github.ReposEditHookParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    testHook(params: Github.ReposTestHookParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    pingHook(params: Github.ReposPingHookParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    deleteHook(params: Github.ReposDeleteHookParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
  };
  search: {
    repos(params: Github.SearchReposParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    code(params: Github.SearchCodeParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    commits(params: Github.SearchCommitsParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    issues(params: Github.SearchIssuesParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    users(params: Github.SearchUsersParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    email(params: Github.SearchEmailParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
  };
  users: {
    get(params: Github.EmptyParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    update(params: Github.UsersUpdateParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    promote(params: Github.UsersPromoteParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    demote(params: Github.UsersDemoteParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    suspend(params: Github.UsersSuspendParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    unsuspend(params: Github.UsersUnsuspendParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getForUser(params: Github.UsersGetForUserParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getById(params: Github.UsersGetByIdParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getAll(params: Github.UsersGetAllParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getOrgs(params: Github.UsersGetOrgsParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getOrgMemberships(params: Github.UsersGetOrgMembershipsParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getOrgMembership(params: Github.UsersGetOrgMembershipParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    editOrgMembership(params: Github.UsersEditOrgMembershipParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getTeams(params: Github.UsersGetTeamsParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getEmails(params: Github.UsersGetEmailsParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getPublicEmails(params: Github.UsersGetPublicEmailsParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    addEmails(params: Github.UsersAddEmailsParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    deleteEmails(params: Github.UsersDeleteEmailsParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    togglePrimaryEmailVisibility(params: Github.EmptyParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getFollowersForUser(params: Github.UsersGetFollowersForUserParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getFollowers(params: Github.UsersGetFollowersParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getFollowingForUser(params: Github.UsersGetFollowingForUserParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getFollowing(params: Github.UsersGetFollowingParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    checkFollowing(params: Github.UsersCheckFollowingParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    checkIfOneFollowersOther(params: Github.UsersCheckIfOneFollowersOtherParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    followUser(params: Github.UsersFollowUserParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    unfollowUser(params: Github.UsersUnfollowUserParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getKeysForUser(params: Github.UsersGetKeysForUserParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getKeys(params: Github.UsersGetKeysParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getKey(params: Github.UsersGetKeyParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    createKey(params: Github.UsersCreateKeyParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    deleteKey(params: Github.UsersDeleteKeyParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getGpgKeysForUser(params: Github.UsersGetGpgKeysForUserParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getGpgKeys(params: Github.UsersGetGpgKeysParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getGpgKey(params: Github.UsersGetGpgKeyParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    createGpgKey(params: Github.UsersCreateGpgKeyParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    deleteGpgKey(params: Github.UsersDeleteGpgKeyParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getBlockedUsers(params: Github.EmptyParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    checkBlockedUser(params: Github.UsersCheckBlockedUserParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    blockUser(params: Github.UsersBlockUserParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    unblockUser(params: Github.UsersUnblockUserParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getRepoInvites(params: Github.EmptyParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    acceptRepoInvite(params: Github.UsersAcceptRepoInviteParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    declineRepoInvite(params: Github.UsersDeclineRepoInviteParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getInstallations(params: Github.UsersGetInstallationsParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getInstallationRepos(params: Github.UsersGetInstallationReposParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    addRepoToInstallation(params: Github.UsersAddRepoToInstallationParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    removeRepoFromInstallation(params: Github.UsersRemoveRepoFromInstallationParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getMarketplacePurchases(params: Github.UsersGetMarketplacePurchasesParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getMarketplaceStubbedPurchases(params: Github.UsersGetMarketplaceStubbedPurchasesParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
  };
  enterprise: {
    stats(params: Github.EnterpriseStatsParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    updateLdapForUser(params: Github.EnterpriseUpdateLdapForUserParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    syncLdapForUser(params: Github.EnterpriseSyncLdapForUserParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    updateLdapForTeam(params: Github.EnterpriseUpdateLdapForTeamParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    syncLdapForTeam(params: Github.EnterpriseSyncLdapForTeamParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getLicense(params: Github.EmptyParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getPreReceiveEnvironment(params: Github.EnterpriseGetPreReceiveEnvironmentParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getPreReceiveEnvironments(params: Github.EmptyParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    createPreReceiveEnvironment(params: Github.EnterpriseCreatePreReceiveEnvironmentParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    editPreReceiveEnvironment(params: Github.EnterpriseEditPreReceiveEnvironmentParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    deletePreReceiveEnvironment(params: Github.EnterpriseDeletePreReceiveEnvironmentParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getPreReceiveEnvironmentDownloadStatus(params: Github.EnterpriseGetPreReceiveEnvironmentDownloadStatusParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    triggerPreReceiveEnvironmentDownload(params: Github.EnterpriseTriggerPreReceiveEnvironmentDownloadParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getPreReceiveHook(params: Github.EnterpriseGetPreReceiveHookParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    getPreReceiveHooks(params: Github.EmptyParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    createPreReceiveHook(params: Github.EnterpriseCreatePreReceiveHookParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    editPreReceiveHook(params: Github.EnterpriseEditPreReceiveHookParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    deletePreReceiveHook(params: Github.EnterpriseDeletePreReceiveHookParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    queueIndexingJob(params: Github.EnterpriseQueueIndexingJobParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
    createOrg(params: Github.EnterpriseCreateOrgParams, callback?: Github.Callback): Promise<Github.AnyResponse>;
  };
}

declare module "octokit-rest-es3" {
  export = Github;
}