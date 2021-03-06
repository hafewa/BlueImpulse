#ifndef ParticleDataType
#define ParticleDataType

struct WorldIData
{
    int num_active_particles;
    uint id_seed;
    int dummy[2];
};

struct WorldData
{
    float timestep;
    float particle_size;
    float particle_lifetime;
    float wall_stiffness;
    float pressure_stiffness;
    float damping;
    float advection;
    int num_max_particles;
    int num_additional_particles;
    int num_sphere_colliders;
    int num_capsule_colliders;
    int num_box_colliders;
    int num_forces;
    float3 world_center;
    float3 world_extents;
    int3 world_div;
    int3 world_div_bits;
    uint3 world_div_shift;
    float3 world_cellsize;
    float3 rcp_world_cellsize;
    float2 rt_size;
    float4x4 view_proj;
    float rcp_particle_size2;
    float3 coord_scaler;
};

struct SPHParams
{
    float smooth_len;
    float particle_mass;
    float pressure_stiffness;
    float rest_density;
    float viscosity;
    float density_coef;
    float pressure_coef;
    float viscosity_coef;
};

struct Particle
{
    float3 position;
    float3 velocity;
    float speed;
    float lifetime;
    float density;
    int hit_objid;
    uint id;
};

struct Cell
{
    int begin;
    int end;
};

struct ParticleIData
{
    float3 accel;
    float affection;
};


struct Sphere
{
    float3 center;
    float radius;
};

struct Capsule
{
    float3 pos1;
    float3 pos2;
    float radius;
};

struct Plane
{
    float3 normal;
    float distance;
};

struct Box
{
    float3 center;
    Plane planes[6];
};

struct AABB
{
    float3 center;
    float3 extents;
};

struct ColliderInfo
{
    int owner_objid;
    AABB aabb;
};


struct SphereCollider
{
    ColliderInfo info;
    Sphere shape;
};

struct CapsuleCollider
{
    ColliderInfo info;
    Capsule shape;
};

struct BoxCollider
{
    ColliderInfo info;
    Box shape;
};


struct ForceInfo
{
    int shape_type; // 0: affect all, 1: sphere, 2: capsule, 3: box
    int dir_type; // 0: directional, 1: radial, 2: vector field
    float strength;
    float random_seed;
    float random_diffuse;
    float3 direction;    // dir_type: directional
    float3 center;       // dir_type: radial
    float3 rcp_cellsize; // dir_type: vector field
};

struct Force
{
    ForceInfo info;
    Sphere sphere;
    Capsule capsule;
    Box box;
};



struct TrailParams
{
    float delta_time;
    float width;
    uint max_entities;
    uint max_history;
    float interval;
    float3 camera_position;
};

struct TrailEntity
{
    uint id;
    float time;
    uint frame;
};

struct TrailHistory
{
    float3 position;
};

struct TrailVertex
{
    float3 position;
    float2 texcoord;
};



float  iq_rand( float  p )
{
    return frac(sin(p)*43758.5453);
}
float2 iq_rand( float2 p )
{
    p = float2( dot(p,float2(127.1,311.7)), dot(p,float2(269.5,183.3)) );
    return frac(sin(p)*43758.5453);
}
float3 iq_rand( float3 p )
{
        p = float3( dot(p,float3(127.1,311.7,311.7)), dot(p,float3(269.5,183.3,183.3)), dot(p,float3(269.5,183.3,183.3)) );
        return frac(sin(p)*43758.5453);
}

#endif // ParticleDataType
