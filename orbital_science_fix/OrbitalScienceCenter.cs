using TUNING;

namespace OrbitalScienceFix {
    public class OrbitalScienceCenter : ComplexFabricator {
        protected override void OnPrefabInit()
        {
            base.OnPrefabInit();
            this.choreType = Db.Get().ChoreTypes.Research;
            this.fetchChoreTypeIdHash = Db.Get().ChoreTypes.ResearchFetch.IdHash;
        }
        protected override void OnSpawn()
        {
            base.OnSpawn();
            this.workable.AttributeConverter = Db.Get().AttributeConverters.ResearchSpeed;
            this.workable.AttributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.MOST_DAY_EXPERIENCE;
            this.workable.SkillExperienceSkillGroup = Db.Get().SkillGroups.Research.Id;
            this.workable.SkillExperienceMultiplier = SKILLS.MOST_DAY_EXPERIENCE;
        }
    }
}