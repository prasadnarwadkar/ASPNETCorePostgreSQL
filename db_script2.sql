-- Table: public.unnaturaldeaths

-- DROP TABLE public.unnaturaldeaths;

CREATE TABLE IF NOT EXISTS public.unnaturaldeaths
(
    "timeOfPostmortemExamination" time(6) with time zone,
    sex character varying(5) COLLATE pg_catalog."default",
    "sceneOfDeath" character varying(200) COLLATE pg_catalog."default",
    "policeStation" character varying(50) COLLATE pg_catalog."default",
    "policeCaseNo" integer,
    "placeOfExamination" character varying(50) COLLATE pg_catalog."default",
    nationality character varying(50) COLLATE pg_catalog."default",
    "informantRelationToDeceased" character varying(50) COLLATE pg_catalog."default",
    "informantName" character varying(50) COLLATE pg_catalog."default",
    "imformantCidNo" character varying(50) COLLATE pg_catalog."default",
    history character varying(300) COLLATE pg_catalog."default",
    "generalExternalInformation" character varying(300) COLLATE pg_catalog."default",
    dzongkhag character varying(50) COLLATE pg_catalog."default",
    "deceasedName" character varying(50) COLLATE pg_catalog."default",
    "dateOfPostmortemExamination" date,
    "cidOrPassport" character varying(50) COLLATE pg_catalog."default",
    age integer,
    address character varying(50) COLLATE pg_catalog."default",
    isactive boolean NOT NULL DEFAULT true,
    transactedby character varying(50) COLLATE pg_catalog."default" NOT NULL,
    transacteddate timestamp with time zone NOT NULL,
    remark character varying(256) COLLATE pg_catalog."default",
    lastchanged timestamp with time zone DEFAULT CURRENT_TIMESTAMP,
    version integer NOT NULL DEFAULT 0,
    id uuid NOT NULL DEFAULT uuid_generate_v4(),
    CONSTRAINT pk_unnatural PRIMARY KEY (id)
)

TABLESPACE pg_default;

ALTER TABLE public.unnaturaldeaths
    OWNER to postgres;
	
-- Table: public.Patient

-- DROP TABLE public."Patient";

CREATE TABLE IF NOT EXISTS public."Patient"
(
    "Name" character varying(100)[] COLLATE pg_catalog."default" NOT NULL,
    "ID" bigint NOT NULL GENERATED ALWAYS AS IDENTITY ( INCREMENT 1 START 1 MINVALUE 1 MAXVALUE 1000000000 CACHE 1 ),
    "Address" character varying(500)[] COLLATE pg_catalog."default" NOT NULL,
    "UHID" uuid,
    "LastModified" timestamp without time zone,
    CONSTRAINT "Patient_pkey" PRIMARY KEY ("ID")
)

TABLESPACE pg_default;

ALTER TABLE public."Patient"
    OWNER to postgres;

COMMENT ON COLUMN public."Patient"."UHID"
    IS 'Unique health ID';